namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class ListCandidateVotedByUser
    {
        public Guid CandidateId { get; set; }
        public string? GroupName { get; set; }
    }
}
