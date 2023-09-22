using Capstone_VotingSystem.Models.ResponseModels.ActivityResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class GetListCandidateByUserIdResponse
    {
        public Guid? CampaignId { get; set; }
        public string CampaignName { get; set; }
        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public string GroupName { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public List<GetActivityByCandidateResponse> ListActivity { get; set; }
    }
}
