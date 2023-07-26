using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.ScoreRequest;
using Capstone_VotingSystem.Models.ResponseModels.ScoreResponse;

namespace Capstone_VotingSystem.Services.ScoreService
{
    public interface IScoreService
    {
        Task<APIResponse<GetScoreResponse>> GetScore(GetScoreByCampaginRequest request);
        Task<APIResponse<GetScoreResponse>> SortScore(GetScoreByCampaginRequest request);
    }
}
