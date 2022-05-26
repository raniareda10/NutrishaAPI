namespace DL.DtosV1.Polls
{
    public class PostAnswerDto
    {
        public long SelectedQuestionId { get; set; }
        public long PollId { get; set; }
    }
}