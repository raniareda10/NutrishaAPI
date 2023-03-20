using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DL.DBContext;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace NutrishaAPI.Jobs
{
    public class CheckSubscriptionsJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CheckSubscriptionsJob(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var currentDate = DateTime.UtcNow.Date;
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            var todaySubscriptionInfo = await appDbContext.SubscriptionInfos
                .AsQueryable()
                .Where(s => s.EndDate != null)
                .Where(s => s.EndDate <= currentDate)
                .Select(s => new
                {
                    s.Id,
                    s.UserId
                }).ToListAsync();

            if (!todaySubscriptionInfo.Any())
            {
                return;
            }
            
            await using var transaction =
                await appDbContext.Database.BeginTransactionAsync(context.CancellationToken);
            var sqlScript = @$"
                    UPDATE MUser Set IsSubscribed = 0, 
                                     SubscriptionDate = null, 
                                     IsManuallySubscribed = 0 
                                 WHERE Id IN ({ParseListIntoInOperator(todaySubscriptionInfo.Select(m => m.UserId))});
                    
                    DELETE FROM SubscriptionInfos WHERE Id IN ({ParseListIntoInOperator(todaySubscriptionInfo.Select(m => m.Id))});";

            await appDbContext.Database.ExecuteSqlRawAsync(sqlScript, context.CancellationToken);
            await transaction.CommitAsync(context.CancellationToken);

            var fireStoreDb = scope.ServiceProvider.GetRequiredService<FirestoreDb>();
            foreach (var userId in todaySubscriptionInfo.Select(m => m.UserId).Distinct())
            {
                try
                {
                    await fireStoreDb.Collection("users").Document(userId.ToString()).DeleteAsync();
                }
                catch (Exception e)
                {
                    // Ignore
                }
            }
        }

        private string ParseListIntoInOperator<T>(IEnumerable<T> list)
        {
            var builder = new StringBuilder();

            foreach (var item in list)
            {
                builder.Append(item.ToString() + ",");
            }

            builder.Remove(builder.Length - 1, 1);
            return builder.ToString();
        }
    }
}