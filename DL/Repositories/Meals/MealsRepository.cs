using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels;
using DL.CommonModels.Paging;
using DL.DBContext;
using DL.EntitiesV1.Meals;
using DL.Extensions;

namespace DL.Repositories.Meals
{
    public class MealsRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;

        public MealsRepository(ICurrentUserService currentUserService, AppDBContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        // public async Task<PagedResult<MealEntity>> GetMealsAsync(GetPagedListQueryModel model)
        // {
        //     var query = _dbContext.Meals.OrderByDescending(m => m.Created).AsQueryable();
        //
        //     if (!string.IsNullOrWhiteSpace(model.SearchWord))
        //     {
        //         query = query.Where(m => m.Name.Contains(model.SearchWord) || m.Label.Contains(model.SearchWord));
        //     }
        //     
        //     return await query.ToPagedListAsync(model);
        // }
        
        
        // public async Task<PagedResult<MealEntity>> PostMealsAsync()
        // {
        //     
        // }
    }
}