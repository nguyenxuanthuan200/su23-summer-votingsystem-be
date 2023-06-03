using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.ResponseModels.VoteResponse;

namespace Capstone_VotingSystem.Repositories.VoteRepo
{
    public interface IVoteRepositories
    {
        Task<VoteDetailResponse> CreateVote(CreateVoteRequest request);
    }
}
