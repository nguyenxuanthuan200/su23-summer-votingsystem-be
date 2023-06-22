namespace Capstone_VotingSystem.Models.ResponseModels.VotingResponse
{
    public class UpdateVoteResponse
    {
        public Guid? VotingId { get; set; }
        public DateTime? SendingTime { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? StateId { get; set; }
        public Guid? FormId { get; set; }
    }
}
