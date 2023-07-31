using Capstone_VotingSystem.Models.RequestModels.ElementRequest;

namespace Capstone_VotingSystem.Models.RequestModels.QuestionRequest
{
    public class UpdateQuestionRequest
    {
        public string? Content { get; set; }
        public Guid? TypeId { get; set; }
        public List<UpdateElementRequest> Element { get; set; }
    }
}
