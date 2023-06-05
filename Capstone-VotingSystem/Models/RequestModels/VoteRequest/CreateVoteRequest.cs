namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class CreateVoteRequest
    {
        public DateTime? Time { get; set; }
        public Guid? CampaignStageId { get; set; }
        public string? UserName { get; set; }
    }
}
