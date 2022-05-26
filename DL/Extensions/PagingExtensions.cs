using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.CommonModels.Paging;
using Microsoft.EntityFrameworkCore;

namespace DL.Extensions
{
    public static class PagingExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, PagedModel pagedModel)
        {
            return query.Skip((pagedModel.PageNumber - 1) * pagedModel.PageSize)
                .Take(pagedModel.PageSize);
        }
        
        public static async Task<PagedResult<T>> ToPagedListAsync<T>(this IQueryable<T> query, PagedModel pagedModel)
        {
            var totalRows = await query.CountAsync();
            return new PagedResult<T>()
            {
                TotalRows = totalRows,
                Data = await query.Skip((pagedModel.PageNumber - 1) * pagedModel.PageSize)
                    .Take(pagedModel.PageSize)
                    .ToListAsync()
            };
        }
    }
}