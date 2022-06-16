using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Reminders.Mobile;
using DL.EntitiesV1.Reminders;
using DL.Repositories.Reminders.Constants;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
                .ToListAsync();

            var reminderGroups = reminderList
                .GroupBy(r => r.ReminderGroupType)
                .Select(grouped => new RemindersListDto()
                {
                    Type = grouped.Key,
                    Reminders = grouped
                        .OrderBy(r => r.Time)
                        .Select(reminder =>
                            new ReminderDetailsDto
                            {
                                Id = reminder.Id,
                                Title = reminder.Title,
                                IsOn = reminder.IsOn,
                                OccurrenceDays =
                                    JsonConvert.DeserializeObject<DayOfWeek[]>(reminder.OccurrenceDays),
                                Time = reminder.Time
                            })
                })
                .OrderBy(g => g.Type)
                .ToList();

            return reminderGroups;
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
                OccurrenceDays = JsonConvert.SerializeObject(postReminderDto.OccurrenceDays),
                Time = postReminderDto.Time
            };

            await _appDbContext.AddAsync(reminder);
            await _appDbContext.SaveChangesAsync();

            return new ReminderDetailsDto()
            {
                Id = reminder.Id,
                Title = reminder.Title,
                IsOn = reminder.IsOn,
                OccurrenceDays = postReminderDto.OccurrenceDays,
                Time = postReminderDto.Time,
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
            reminder.OccurrenceDays = JsonConvert.SerializeObject(putReminderDto.OccurrenceDays);
            reminder.Time = putReminderDto.Time;

            _appDbContext.Update(reminder);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task GetTodayReminders()
        {
            var q = await _appDbContext.Reminders
                .Where(r =>
                    r.IsOn && r.OccurrenceDays.Contains(DateTime.UtcNow.DayOfWeek.ToString("D")))
                .Select(s => new
                {
                    s.Title, Time = s.Time
                }).ToListAsync();


            var list = new List<dynamic>();
            q.ForEach(r =>
            {
                dynamic d = new ExpandoObject();
                // d.Time = DateTime.Today.AddHours(r.Time.Hour).AddMinutes(r.Time.Minute);
                d.Title = r.Title;
            });
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

            var days = Enum.GetValues<DayOfWeek>();
            var created = DateTime.UtcNow;

            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Meals,
                UserId = userId,
                Time = DefaultRemindersTime.BreakFastTime,
                OccurrenceDays = JsonConvert.SerializeObject(days),
                Title = ReminderConstants.BreakFast
            });
            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Meals,
                UserId = userId,
                Time = DefaultRemindersTime.DinnerTime,
                OccurrenceDays = JsonConvert.SerializeObject(days),
                Title = ReminderConstants.Dinner
            });

            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Meals,
                UserId = userId,
                Time = DefaultRemindersTime.LunchTime,
                OccurrenceDays = JsonConvert.SerializeObject(days),
                Title = ReminderConstants.Lunch
            });

            reminders.Add(new ReminderEntity()
            {
                Created = created,
                ReminderGroupType = ReminderGroupType.Weight,
                UserId = userId,
                Time = DefaultRemindersTime.CheckWeightTime,
                OccurrenceDays = JsonConvert.SerializeObject(new DayOfWeek[]
                {
                    DayOfWeek.Monday
                })
            });

            await _appDbContext.AddRangeAsync(reminders);
            await _appDbContext.SaveChangesAsync();
        }
    }
}