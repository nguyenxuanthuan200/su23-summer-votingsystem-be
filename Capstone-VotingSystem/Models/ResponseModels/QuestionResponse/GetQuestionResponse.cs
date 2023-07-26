using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.QuestionResponse
{
    public class GetQuestionResponse
    {
        public Guid QuestionId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? FormId { get; set; }
        public string? TypeName { get; set; }
        public List<GetElementResponse> Element { get; set; }
    }
}
