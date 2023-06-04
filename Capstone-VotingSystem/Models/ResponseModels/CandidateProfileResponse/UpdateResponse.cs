namespace Capstone_VotingSystem.Models.ResponseModels.CandidateProfile
{
    public class UpdateResponse
    {
        public Guid CandidateProfileId { get; set; }
        public string? NickName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Image { get; set; }
    }
}
