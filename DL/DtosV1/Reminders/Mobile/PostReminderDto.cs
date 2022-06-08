using System;
using DL.EntitiesV1.Enum;
using DL.EntitiesV1.Reminders;

namespace DL.DtosV1.Reminders.Mobile
{
    public class PostReminderDto
    {
        public int ReminderGroupId { get; set; }
        public string Title { get; set; }
        public DaysEnum[] OccurrenceDays { get; set; }
        public ReminderType ReminderType { get; set; }
        public DateTime Time { get; set; }
    }
}