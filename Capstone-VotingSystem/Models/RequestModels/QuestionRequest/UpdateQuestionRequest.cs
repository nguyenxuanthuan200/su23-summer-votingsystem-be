using Capstone_VotingSystem.Models.RequestModels.ElementRequest;

namespace Capstone_VotingSystem.Models.RequestModels.QuestionRequest
{
    public class UpdateQuestionRequest
    {
        public string? QuestionName { get; set; }
        public Guid? QuestionTypeId { get; set; }
        public List<UpdateElementRequest> Element { get; set; }
    }
}
