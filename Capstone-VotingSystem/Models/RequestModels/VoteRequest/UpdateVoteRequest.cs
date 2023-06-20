namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class UpdateVoteRequest
    {
        public DateTime? SendingTime { get; set; }
        public Guid? CandidateId { get; set; }

        public Guid? StateId { get; set; }
    }
}
