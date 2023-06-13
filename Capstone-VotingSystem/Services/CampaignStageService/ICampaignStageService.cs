using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse;

namespace Capstone_VotingSystem.Services.CampaignStageService
{
    public interface ICampaignStageService
    {
        Task<APIResponse<IEnumerable<GetCampaignStageByCampaignResponse>>> GetCampaignStageByCampaign(Guid campaignId);
        Task<APIResponse<CreateCampaginStageResponse>> CreateCampaignStage(CreateCampaignStageRequest request);
        Task<APIResponse<GetCampaignStageByCampaignResponse>> UpdateCampaignStage(Guid id,UpdateCampaignStageRequest request);
    }
}
