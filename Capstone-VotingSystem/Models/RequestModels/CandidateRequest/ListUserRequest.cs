namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class ListUserRequest
    {
        public string? Description { get; set; }
        public string UserId { get; set; }
        public Guid GroupId { get; set; }
    }
}
