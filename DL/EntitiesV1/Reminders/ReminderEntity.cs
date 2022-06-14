using System;
using DL.Entities;
using DL.EntitiesV1.Enum;

namespace DL.EntitiesV1.Reminders
{
    public class ReminderEntity : BaseEntityV1
    {
        public string Title { get; set; }
        
        public ReminderGroupType ReminderGroupType { get; set; }
        public int UserId { get; set; }
        public MUser User { get; set; }
        public DayOfWeek[] OccurrenceDays { get; set; }
        public bool IsOn { get; set; }
        public DateTime Time { get; set; }
    }
    
    public enum ReminderGroupType
    {
        Meals = 0,
        Weight = 1,
        Other = 2
    }
}