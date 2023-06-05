namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class CreateAccountCandidateResponse
    {
        public string UserName { get; set; } = null!;
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? Name { get; set; }
    }
}
