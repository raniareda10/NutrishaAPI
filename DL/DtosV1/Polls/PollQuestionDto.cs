using DL.EntitiesV1.Blogs.Polls;

namespace DL.DtosV1.Polls
{
    public class PollQuestionDto
    {
        public long Id { get; set; }
        public string Content { get; set; }
        public int SelectedCount { get; set; }

        public static PollQuestionDto FromPollQuestion(PollQuestion question)
        {
            return new PollQuestionDto()
            {
                Id = question.Id,
                Content = question.Content,
                SelectedCount = question.SelectedCount
            };
        }
    }
}