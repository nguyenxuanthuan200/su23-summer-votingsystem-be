namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class UpdateCandidateProfileResponse
    {
        public Guid CandidateProfileId { get; set; }
        public string? NickName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Image { get; set; }
        public Guid? CampaignId { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
