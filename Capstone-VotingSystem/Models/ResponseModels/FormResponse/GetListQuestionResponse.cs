using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.FormResponse
{
    public class GetListQuestionResponse
    {
        public Guid QuestionId { get; set; }
        public string? Content { get; set; }
        public Guid TypeId { get; set; }

        public List<ListElementQuestionResponse> Elements { get; set; }
    }
}
