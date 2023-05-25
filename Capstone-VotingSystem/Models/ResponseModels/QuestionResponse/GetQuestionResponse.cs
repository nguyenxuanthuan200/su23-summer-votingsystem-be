namespace Capstone_VotingSystem.Models.ResponseModels.QuestionResponse
{
    public class GetQuestionResponse
    {
        public Guid QuestionId { get; set; }
        public string? QuestionOfCampaign { get; set; }
        public string? Description { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
