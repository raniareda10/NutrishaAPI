using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Logging;
using DL.DBContext;
using Google.Cloud.Firestore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;

namespace NutrishaAPI.Jobs
{
    public class CheckSubscriptionsJob : IJob
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<CheckSubscriptionsJob> _logger;

        public CheckSubscriptionsJob(IServiceScopeFactory serviceScopeFactory, ILogger<CheckSubscriptionsJob> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var currentDate = DateTime.UtcNow;
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDBContext>();

            _logger.LogInformation("CheckSubscriptionsJob: Start Execute CheckSubscriptionsJob at {0}",
                currentDate);
            var todaySubscriptionInfo = await appDbContext.SubscriptionInfos
                .AsQueryable()
                .Where(s => s.EndDate != null)
                .Where(s => s.EndDate <= currentDate)
                .Select(s => new
                {
                    s.Id,
                    s.UserId
                }).ToListAsync();

            _logger.LogInformation("CheckSubscriptionsJob: No of Manual Subscriptions Fetched {0}",
                todaySubscriptionInfo.Count);
            if (!todaySubscriptionInfo.Any())
            {
                return;
            }

            _logger.LogInformation("CheckSubscriptionsJob: Start Delete User From Firestore database");
            var fireStoreDb = scope.ServiceProvider.GetRequiredService<FirestoreDb>();
            foreach (var userId in todaySubscriptionInfo.Select(m => m.UserId).Distinct())
            {
                _logger.LogInformation("CheckSubscriptionsJob: FireStore Deleting User {0}", userId);
                var result = await fireStoreDb.Collection("users").Document(userId.ToString()).DeleteAsync();
                _logger.LogInformation("CheckSubscriptionsJob: Deleting FireStore  User  Result {0}",
                    JsonConvert.SerializeObject(result));
            }

            await using var transaction =
                await appDbContext.Database.BeginTransactionAsync(context.CancellationToken);
            var sqlScript = @$"
                    UPDATE MUser Set IsSubscribed = 0, 
                                     SubscriptionDate = null, 
                                     IsManuallySubscribed = 0 
                                 WHERE Id IN ({ParseListIntoInOperator(todaySubscriptionInfo.Select(m => m.UserId))});
                    
                    DELETE FROM SubscriptionInfos WHERE Id IN ({ParseListIntoInOperator(todaySubscriptionInfo.Select(m => m.Id))});";

            var numberOfRowEffected =
                await appDbContext.Database.ExecuteSqlRawAsync(sqlScript, context.CancellationToken);
            _logger.LogInformation("CheckSubscriptionsJob: No of Row Effected {0}", numberOfRowEffected);
            await transaction.CommitAsync(context.CancellationToken);
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