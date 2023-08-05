using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.StageRequest;
using Capstone_VotingSystem.Models.ResponseModels.StageResponse;

namespace Capstone_VotingSystem.Services.StageService
{
    public interface IStageService
    {
        Task<APIResponse<IEnumerable<GetStageResponse>>> GetCampaignStageByCampaign(Guid campaignId);
        Task<APIResponse<CreateStageResponse>> CreateCampaignStage(CreateStageRequest request);
        Task<APIResponse<GetStageResponse>> UpdateCampaignStage(Guid id,UpdateStageRequest request);
        Task<APIResponse<string>> DeleteStage(Guid id);
    }
}
