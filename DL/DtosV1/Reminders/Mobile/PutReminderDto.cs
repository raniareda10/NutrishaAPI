using System;
using DL.EntitiesV1.Enum;
using DL.EntitiesV1.Reminders;

namespace DL.DtosV1.Reminders.Mobile
{
    public class PutReminderDto
    {
        public int ReminderId { get; set; }
        public DaysEnum[] OccurrenceDays { get; set; }
        public ReminderType ReminderType { get; set; }
        public DateTime Time { get; set; }
    }
}