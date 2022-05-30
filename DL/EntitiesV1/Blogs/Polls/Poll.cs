using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DL.EntitiesV1.Blogs.Polls
{
    public class Poll
    {
        [ForeignKey(nameof(Blog))] public long Id { get; set; }
        public Blog Blog { get; set; }

        public IList<PollQuestion> Questions { get; set; }
        public string BackgroundColor { get; set; }
    }
}