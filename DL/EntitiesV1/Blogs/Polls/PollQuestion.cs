namespace DL.EntitiesV1.Blogs.Polls
{
    public class PollQuestion : BaseEntityV1
    {
        public long PollId { get; set; }
        public Poll Poll { get; set; }
        public string Content { get; set; }
    }
}