using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DL.DBContext;
using DL.DtosV1.Reminders.Mobile;
using DL.EntitiesV1.Reminders;
using Microsoft.EntityFrameworkCore;

namespace DL.Services.Reminders
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
            return await _appDbContext.ReminderGroups
                .OrderBy(group => group.Order)
                .Select(group => new RemindersListDto
                {
                    Group = new ReminderGroupDto
                    {
                        Id = group.Id,
                        Title = group.Title,
                        Order = group.Order
                    },
                    Reminders = group.Reminders
                        .OrderBy(r => r.Time)
                        .Where(r => r.UserId == _currentUserService.UserId).Select(reminder =>
                            new ReminderDetailsDto
                            {
                                Id = reminder.Id,
                                Title = reminder.Title,
                                IsOn = reminder.IsOn,
                                OccurrenceDays = reminder.OccurrenceDays,
                                ReminderType = reminder.ReminderType,
                                Time = reminder.Time
                            })
                }).ToListAsync();
        }

        public async Task<ReminderDetailsDto> PostAsync(PostReminderDto postReminderDto)
        {
            var reminder = new ReminderEntity()
            {
                Created = DateTime.UtcNow,
                UserId = _currentUserService.UserId,
                ReminderType = postReminderDto.ReminderType,
                IsOn = true,
                ReminderGroupId = postReminderDto.ReminderGroupId,
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
                ReminderType = reminder.ReminderType,
                Time = reminder.Time
            };
        }

        public async Task PutAsync(PutReminderDto putReminderDto)
        {
            var reminder = await _appDbContext.Reminders
                .Include(r => r.ReminderGroup)
                .Where(r => r.UserId == _currentUserService.UserId && r.Id == putReminderDto.ReminderId)
                .FirstOrDefaultAsync();

            if (reminder == null)
            {
                return;
            }

            reminder.OccurrenceDays = putReminderDto.OccurrenceDays;
            reminder.Time = putReminderDto.Time;
            reminder.ReminderType = putReminderDto.ReminderType;

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
    }
}