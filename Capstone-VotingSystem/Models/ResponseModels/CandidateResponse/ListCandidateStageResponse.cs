namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class ListCandidateStageResponse
    {
        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public string? UserId { get; set; }

        public Guid? GroupId { get; set; }
        public string? GroupName { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }

        public int? StageScore { get; set; }
        public bool isVoted { get; set; } = false;
    }
}
