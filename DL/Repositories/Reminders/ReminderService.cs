using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Reminders.Mobile;
using DL.EntitiesV1.Reminders;
using Microsoft.EntityFrameworkCore;

namespace DL.Repositories.Reminders
{
    public class ReminderService
    {
        private readonly AppDBContext _appDbContext;
        private readonly ICurrentUserService _currentUserService;

        public ReminderService(AppDBContext appDbContext, ICurrentUserService currentUserService)
        {
            _appDbContext = appDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<RemindersListDto>> GetAllAsync()
        {
            var reminderList = await _appDbContext.Reminders
                .Where(r => r.UserId == _currentUserService.UserId)
                .OrderByDescending(r => r.Time)
                .ToListAsync();
            
            return reminderList.GroupBy(r => r.ReminderGroupType)
                .Select(grouped => new RemindersListDto()
                {
                    Type = grouped.Key,
                    Reminders = grouped.Select(reminder =>
                        new ReminderDetailsDto
                        {
                            Id = reminder.Id,
                            Title = reminder.Title,
                            IsOn = reminder.IsOn,
                            OccurrenceDays = reminder.OccurrenceDays,
                            Time = reminder.Time
                        })
                }).ToList();
        }

        public async Task<ReminderDetailsDto> PostAsync(PostReminderDto postReminderDto)
        {
            var reminder = new ReminderEntity()
            {
                Created = DateTime.UtcNow,
                ReminderGroupType = postReminderDto.GroupType,
                UserId = _currentUserService.UserId,
                IsOn = true,
                Title = postReminderDto.Title,
                OccurrenceDays = postReminderDto.OccurrenceDays,
                Time = postReminderDto.Time
            };

            await _appDbContext.AddAsync(reminder);
            await _appDbContext.SaveChangesAsync();

            return new ReminderDetailsDto()
            {
                Id = reminder.Id,
                Title = reminder.Title,
                IsOn = reminder.IsOn,
                OccurrenceDays = reminder.OccurrenceDays,
                Time = reminder.Time
            };
        }

        public async Task PutAsync(PutReminderDto putReminderDto)
        {
            var reminder = await _appDbContext.Reminders
                .Where(r => r.UserId == _currentUserService.UserId && r.Id == putReminderDto.ReminderId)
                .FirstOrDefaultAsync();

            if (reminder == null)
            {
                return;
            }

            reminder.Title = putReminderDto.Title;
            reminder.OccurrenceDays = putReminderDto.OccurrenceDays;
            reminder.Time = putReminderDto.Time;

            _appDbContext.Update(reminder);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task TurnOnAsync(TurnReminderOnDto reminderOnDto)
        {
            var reminder = await _appDbContext.Reminders
                .Where(r => r.UserId == _currentUserService.UserId && r.Id == reminderOnDto.ReminderId)
                .FirstOrDefaultAsync();

            if (reminder == null)
            {
                return;
            }

            reminder.IsOn = reminderOnDto.TurnOn;
            _appDbContext.Update(reminder);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task CreateDefaultRemindersAsync(int userId)
        {
            const int defaultReminderCount = 4;
            var reminders = new List<ReminderEntity>(defaultReminderCount);

            var created = DateTime.UtcNow;
            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Meals,
                UserId = userId,
                Time = DateTime.UtcNow,
                Title = ReminderConstants.BreakFast
            });
            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Meals,
                UserId = userId,
                Time = DateTime.UtcNow,
                Title = ReminderConstants.Dinner
            });

            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Meals,
                UserId = userId,
                Time = DateTime.UtcNow,
                Title = ReminderConstants.Lunch
            });

            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Weight,
                UserId = userId,
                Time = DateTime.UtcNow,
                Title = ReminderConstants.BreakFast
            });

            await _appDbContext.AddRangeAsync(reminders);
            await _appDbContext.SaveChangesAsync();
        }
    }
}