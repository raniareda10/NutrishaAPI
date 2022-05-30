using System.Collections.Generic;

namespace DL.DtosV1.Polls
{
    public class PostPollDto
    {
        public string Question { get; set; }
        public List<string> Answers { get; set; }
        public long TagId { get; set; }
    }
}