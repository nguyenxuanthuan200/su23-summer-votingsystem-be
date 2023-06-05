namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateCandidateCampaignRequest
    {
        public string? NickName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Image { get; set; }
        public string UserName { get; set; }
        public Guid CampaignId { get; set; }
    }
}
