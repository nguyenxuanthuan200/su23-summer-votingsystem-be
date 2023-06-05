namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateAccountCandidateRequest
    {
        public string UserName { get; set; } = null!;
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
