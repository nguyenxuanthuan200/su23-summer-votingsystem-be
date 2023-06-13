using Capstone_VotingSystem.Models.RequestModels.ElementRequest;

namespace Capstone_VotingSystem.Models.RequestModels.QuestionRequest
{
    public class CreateQuestionRequest
    {
        public string? QuestionName { get; set; }
        public Guid? FormId { get; set; }
        public Guid? QuestionTypeId { get; set; }
        public List<CreateElementRequest> Element { get; set; }
    }
}
