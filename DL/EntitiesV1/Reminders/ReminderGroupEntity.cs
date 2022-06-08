using System.Collections.Generic;

namespace DL.EntitiesV1.Reminders
{
    public class ReminderGroupEntity
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Order { get; set; }
        
        public IList<ReminderEntity> Reminders { get; set; }
    }
}