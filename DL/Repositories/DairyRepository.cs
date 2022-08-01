using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Dairies;
using DL.EntitiesV1;
using DL.EntitiesV1.Dairies;
using DL.EntitiesV1.Meals;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories
{
    public class DairyRepository
    {
        private readonly AppDBContext _dbContext;
        private readonly ICurrentUserService _currentUserService;

        public DairyRepository(
            AppDBContext dbContext,
            ICurrentUserService currentUserService)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
        }

        public async Task<long> PostAsync(CreateDairyDto dto)
        {
            var dairyEntity = new DairyEntity()
            {
                Created = DateTime.UtcNow,
                Details = dto.Details,
                Name = dto.Name,
                Type = dto.Type,
                UserId = _currentUserService.UserId
            };

            await _dbContext.Dairies.AddAsync(dairyEntity);
            await _dbContext.SaveChangesAsync();
            return dairyEntity.Id;
        }

        public async Task<IDictionary<int, IEnumerable<GetDairiesDto>>> GetTodayDairiesAsync()
        {
            var dairies = await _dbContext.Dairies
                .AsNoTracking()
                .Where(d => d.UserId == _currentUserService.UserId)
                .Where(GetTodayFilter<DairyEntity>())
                .ToListAsync();

            return dairies.GroupBy(d => d.Type)
                .ToDictionary(d => (int)d.Key,
                    d => d
                        .OrderByDescending(d => d.Name)
                        .Select(m => new GetDairiesDto
                        {
                            Id = m.Id,
                            Details = m.Details,
                            Name = m.Name,
                            Type = m.Type
                        }));
        }

        private Expression<Func<T, bool>> GetTodayFilter<T>() where T : BaseEntityV1
        {
            var yesterday = DateTime.UtcNow.AddHours(_currentUserService.UserTimeZoneDifference).Date;
            var tomorrow = yesterday.AddDays(1);

            return d => d.Created.AddHours(_currentUserService.UserTimeZoneDifference) >= yesterday &&
                        d.Created.AddHours(_currentUserService.UserTimeZoneDifference) <= tomorrow;
        }
    }
}