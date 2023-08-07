namespace Capstone_VotingSystem.Models.RequestModels.QuestionRequest
{
    public class CreateQuestionWithNoElementRequest
    {
        public string Content { get; set; }
        public Guid FormId { get; set; }
        public Guid TypeId { get; set; }
        public int Score { get; set; }
    }
}
