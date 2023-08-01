namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class ListAccountCandidateRequest
    {
        public string UserName { get; set; } = null!;
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public Guid GroupId { get; set; }
    }
}
