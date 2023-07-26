namespace Capstone_VotingSystem.Models.ResponseModels.QuestionResponse
{
    public class GetQuestionNoElementResponse
    {
        public Guid QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? FormId { get; set; }
        public string? TypeName { get; set; }
    }
}
