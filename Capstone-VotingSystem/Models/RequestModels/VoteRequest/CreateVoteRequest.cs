using Capstone_VotingSystem.Models.RequestModels.ElementRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;

namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class CreateVoteRequest
    {
        public DateTime? SendingTime { get; set; }
        //public Guid? RatioGroupId { get; set; }
        public string? UserId { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? StageId { get; set; }

        public List<CreateVoteDetailRequest> VotingDetail { get; set; }
    }
}
