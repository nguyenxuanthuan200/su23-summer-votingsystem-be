using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.RequestModels.VotingDetailRequest;
using Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse;
using Capstone_VotingSystem.Models.ResponseModels.VotingResponse;


namespace Capstone_VotingSystem.Services.VoteService
{
    public interface IVoteService
    {
        Task<APIResponse<string>> CreateVote(CreateVoteRequest request);
        Task<APIResponse<string>> CreateVoteLike(CreateVoteLikeRequest request);
        //Task<APIResponse<string>> UndoVote(CreateVoteLikeRequest request);
        Task<APIResponse<IEnumerable<SatisticalVoteInCampaignResponse>>> StatisticalVoteByCampaign(StatisticalVoteRequest request);
    }
}
