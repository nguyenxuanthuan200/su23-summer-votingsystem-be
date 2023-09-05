namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class CreateCandidateRequest
    {
        public string? Description { get; set; }
        public string FullName { get; set; }
        public string GroupName { get; set; }
    }
}
