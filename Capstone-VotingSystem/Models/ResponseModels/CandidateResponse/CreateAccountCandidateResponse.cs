namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class CreateAccountCandidateResponse
    {
        public string UserId { get; set; } = null!;
        public string? Address { get; set; }
        public string? FullName { get; set; }
    }
}
