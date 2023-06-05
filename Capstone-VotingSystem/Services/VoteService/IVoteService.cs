using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
namespace Capstone_VotingSystem.Repositories.VoteRepo
{
    public interface IVoteService
    {
        Task<Boolean> CreateVote(CreateVoteRequest request);

        Task<Boolean> CreateVoteDetail(CreateVoteDetailRequest request);
    }
}
