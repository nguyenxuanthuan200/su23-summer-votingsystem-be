namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateAccountCandidateRequest
    {
        public string UserName { get; set; } = null!;
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public string? Address { get; set; }
        public Guid GroupId { get; set; }
        public Guid CampaignId { get; set; }
    }
}
