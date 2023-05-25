using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
namespace Capstone_VotingSystem.Repositories.VoteRepo
{
    public interface IVoteRepositories
    {
        Task<Boolean> CreateVote(CreateVoteRequest request);
    }
}
