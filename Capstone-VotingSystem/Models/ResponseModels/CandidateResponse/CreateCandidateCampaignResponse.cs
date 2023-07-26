namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class CreateCandidateCampaignResponse
    {
        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
