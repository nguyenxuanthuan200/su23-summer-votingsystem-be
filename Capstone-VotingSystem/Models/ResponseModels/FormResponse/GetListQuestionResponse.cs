namespace Capstone_VotingSystem.Models.ResponseModels.FormResponse
{
    public class GetListQuestionResponse
    {
        public Guid QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid TypeId { get; set; }
    }
}
