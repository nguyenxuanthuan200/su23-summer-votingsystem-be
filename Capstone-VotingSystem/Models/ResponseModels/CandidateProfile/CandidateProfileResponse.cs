namespace Capstone_VotingSystem.Models.ResponseModels.CandidateProfile
{
    public class CandidateProfileResponse
    {
        public Guid CandidateProfileId { get; set; }
        public string? NickName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Image { get; set; }
        public string? Username { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
