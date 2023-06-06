using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;

namespace Capstone_VotingSystem.Services.VoteService
{
    public interface IVoteService
    {
        Task<bool> CreateVote(CreateVoteRequest request);

        Task<bool> CreateVoteDetail(CreateVoteDetailRequest request);
    }
}
