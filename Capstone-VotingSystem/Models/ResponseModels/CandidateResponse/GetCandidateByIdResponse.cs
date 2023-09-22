namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class GetCandidateByIdResponse
    {
        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public string? AvatarUrl { get; set; }
        public string? GroupId { get; set; }
    }
}
