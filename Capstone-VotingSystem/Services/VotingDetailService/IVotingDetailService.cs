using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse;

namespace Capstone_VotingSystem.Repositories.VoteRepo
{
    public interface IVotingDetailService
    {
        public Task<VoteDetailResponse> CreateVotingDetail(CreateVoteDetailRequest request);
        public Task<IEnumerable<VoteDetailResponse>> GetAll();
    }
}
