using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.VotingResponse
{
    public class CreateVoteResponse
    {
        public Guid VoringId { get; set; }
        public DateTime? SendingTime { get; set; }
        public bool? Status { get; set; }
        public Guid? RatioGroupId { get; set; }
        public string? UserId { get; set; }
        public Guid? CandidateId { get; set; }
        public Guid? StageId { get; set; }

        public List<CreateVoteDetailResponse> VoteDetails { get; set; }
    }
}
