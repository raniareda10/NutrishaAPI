using System;
using DL.Entities;
using DL.EntitiesV1.Enum;

namespace DL.EntitiesV1.Reminders
{
    public class ReminderEntity : BaseEntityV1
    {
        public string Title { get; set; }
        public long ReminderGroupId { get; set; }
        public ReminderGroupEntity ReminderGroup { get; set; }
        public ReminderType ReminderType { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
        public DaysEnum[] OccurrenceDays { get; set; }
        public bool IsOn { get; set; }
        public DateTime Time { get; set; }
    }

    public enum ReminderType
    {
        Once = 0,
        Daily = 1,
        Weekly = 2,
        Monthly = 3
    }
}