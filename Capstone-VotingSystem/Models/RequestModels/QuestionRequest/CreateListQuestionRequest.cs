namespace Capstone_VotingSystem.Models.RequestModels.QuestionRequest
{
    public class CreateListQuestionRequest
    {
        public Guid FormId { get; set; }
        public List<CreateQuestionRequest> listQuestion { get; set; }
    }
}
