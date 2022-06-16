using System;
using System.Collections.Generic;
using DL.EntitiesV1.Enum;
using DL.EntitiesV1.Reminders;
using Newtonsoft.Json;

namespace DL.DtosV1.Reminders.Mobile
{
    public class RemindersListDto
    {
        public ReminderGroupType Type { get; set; }
        public IEnumerable<ReminderDetailsDto> Reminders { get; set; }
    }

    public class ReminderDetailsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DayOfWeek[] OccurrenceDays { get; set; }
        
        [JsonIgnore]
        public string OccurrenceDaysMap { get; set; }
        public bool IsOn { get; set; }
        public TimeSpan Time { get; set; }
    }
}