namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class CreateVoteLikeRequest
    {
        public DateTime SendingTime { get; set; }
        public string UserId { get; set; }
        public Guid CandidateId { get; set; }
        public Guid StageId { get; set; }
    }
}
