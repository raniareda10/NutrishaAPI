using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.UserMeasurements;
using DL.EntitiesV1.Measurements;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.UserMeasurement
{
    public class UserMeasurementRepository
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly AppDBContext _dbContext;

        public UserMeasurementRepository(ICurrentUserService currentUserService, AppDBContext dbContext)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
        }

        public async Task<long> PostAsync(PostUserMeasurement postUserMeasurement)
        {
            var measurement = new UserMeasurementEntity()
            {
                Created = DateTime.UtcNow,
                MeasurementType = postUserMeasurement.MeasurementType,
                MeasurementValue = postUserMeasurement.MeasurementValue,
                UserId = _currentUserService.UserId
            };

            await _dbContext.AddAsync(measurement);
            await _dbContext.SaveChangesAsync();

            return measurement.Id;
        }

        public async Task<UserMeasurementDetailsDto> GetLastMeasurementDetailsAsync(MeasurementType type)
        {
            return await _dbContext.UserMeasurements
                .Where(m => m.MeasurementType == type && m.UserId == _currentUserService.UserId)
                .OrderByDescending(p => p.Created)
                .Select(m => new UserMeasurementDetailsDto
                {
                    MeasurementValue = m.MeasurementValue
                })
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<UserMeasurements>> GetMeasurementsListAsync(
            IList<MeasurementType> measurementTypes)
        {
            var fromOneMonthAgo = DateTime.UtcNow.AddMonths(-1);
            var measurements = await _dbContext.UserMeasurements
                .Where(m => m.UserId == _currentUserService.UserId)
                .Where(m => m.Created >= fromOneMonthAgo)
                .Where(m => measurementTypes.Contains(m.MeasurementType))
                .OrderByDescending(m => m.Created)
                .Select(m => new
                {
                    m.MeasurementType,
                    Created = m.Created,
                    MeasurementValue = m.MeasurementValue
                })
                .ToListAsync();

            return measurements.GroupBy(m => m.MeasurementType)
                .Select(grouping => new UserMeasurements()
                {
                    Type = grouping.Key,
                    Measurements = grouping.Select(m => new UserMeasurementListDto()
                    {
                        Created = m.Created,
                        MeasurementValue = m.MeasurementValue
                    })
                });
        }

        public async Task<IEnumerable<UserMeasurementListDto>> GetMeasurementsListForOneTypeOnlyAsync(
            MeasurementType type)
        {
            return await _dbContext.UserMeasurements
                .Where(m => m.UserId == _currentUserService.UserId)
                .Where(m => m.MeasurementType == type)
                .OrderByDescending(p => p.Created)
                .Select(m => new UserMeasurementListDto
                {
                    Created = m.Created,
                    MeasurementValue = m.MeasurementValue
                })
                .ToListAsync();
        }

        public async Task PostMultiMeasurementsAsync(IEnumerable<PostUserMeasurement> measurements)
        {
            var entities = measurements.Select(m => new UserMeasurementEntity()
            {
                Created = DateTime.UtcNow,
                MeasurementType = m.MeasurementType,
                MeasurementValue = m.MeasurementValue,
                UserId = _currentUserService.UserId
            });
            
            await _dbContext.UserMeasurements.AddRangeAsync(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}