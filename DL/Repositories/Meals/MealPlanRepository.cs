using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.DtosV1.Common;
using DL.DtosV1.Meals;
using DL.EntitiesV1.Meals;
using DL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Meals
{
    public class MealPlanRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;

        public MealPlanRepository(ICurrentUserService currentUserService, AppDBContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<long> PostMealPlanAsync(PostMealPlanDto plans)
        {
            if (plans.IsTemplate)
            {
                plans.UserId = null;
            }

            var currentDate = DateTime.UtcNow;
            var plan = new MealPlanEntity
            {
                CreatedById = _currentUserService.UserId,
                UserId = plans.UserId,
                IsTemplate = plans.IsTemplate,
                TemplateName = plans.TemplateName,
                Notes = plans.Notes,
                Created = currentDate,
                PlanDays = plans.Meals.Select(m => new PlanDayEntity()
                {
                    Created = currentDate,
                    Day = m.Day,
                    PlanMeals = m.Menus.Select(menu => new PlanDayMenuEntity
                    {
                        Created = currentDate,
                        MealType = menu.Type,
                        Meals = menu.MealIds.Select(mealId => new PlanDayMenuMealEntity()
                        {
                            Created = currentDate,
                            MealId = mealId
                        }).ToList()
                    }).ToList(),
                }).ToList(),
                StartDate = plans.StartDate,
            };
            
            await _dbContext.AddAsync(plan);
            await _dbContext.SaveChangesAsync();

            return plan.Id;
        }

        public async Task<PagedResult<LookupItem>> GetTemplatePagedListAsync(GetPagedListQueryModel model)
        {
            var query = _dbContext.MealPlans.Where(p => p.IsTemplate)
                .OrderByDescending(p => p.Created)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(model.SearchWord))
            {
                query = query.Where(p => p.TemplateName.Contains(model.SearchWord));
            }

            return await query
                .Select(m => new LookupItem(m.Id, m.TemplateName))
                .ToPagedListAsync(model);
        }

        public async Task<object> GetTemplateByIdAsync(long id)
        {
            var mealPlans = await _dbContext.MealPlans.AsNoTracking()
                .Include(m => m.PlanDays)
                .ThenInclude(m => m.PlanMeals)
                .ThenInclude(m => m.Meals)
                .ThenInclude(m => m.Meal)
                .FirstOrDefaultAsync(p => p.Id == id);

            return mealPlans;
        }

        public async Task<long> UpdateTemplateAsync(UpdateMealPlan updateMealPlan)
        {
            var effectedRowsCount = await _dbContext.Database.ExecuteSqlRawAsync(
                $"DELETE FROM MealPlans WHERE Id = {updateMealPlan.Id} AND IsTemplate = 1");

            if (effectedRowsCount == 0)
            {
                return 0;
            }
            
            return await PostMealPlanAsync(updateMealPlan);
        }
    }
}