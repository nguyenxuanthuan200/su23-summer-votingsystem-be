namespace Capstone_VotingSystem.Models.ResponseModels.QuestionResponse
{
    public class GetQuestionResponse
    {
        public Guid Id { get; set; }
        public string? Question1 { get; set; }
        public string? Description { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
