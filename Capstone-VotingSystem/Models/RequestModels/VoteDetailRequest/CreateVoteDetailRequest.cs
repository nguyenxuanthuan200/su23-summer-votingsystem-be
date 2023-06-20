namespace Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest
{
    public class CreateVoteDetailRequest
    {
        public DateTime? CreateTime { get; set; }
        public Guid? ElementId { get; set; }
        public Guid? VotingId { get; set; }
    }
}
