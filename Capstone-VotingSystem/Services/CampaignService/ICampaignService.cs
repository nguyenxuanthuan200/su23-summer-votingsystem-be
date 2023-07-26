using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;

namespace Capstone_VotingSystem.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<APIResponse<IEnumerable<GetCampaignAndStageResponse>>> GetCampaign();
        Task<APIResponse<GetCampaignResponse>> UpdateCampaign(Guid id,UpdateCampaignRequest request);
        Task<APIResponse<GetCampaignResponse>> UpdateVisibilityCampaign(Guid id, string request,string us);
        Task<APIResponse<GetCampaignResponse>> CreateCampaign(CreateCampaignRequest request);
        Task<APIResponse<GetCampaignAndStageResponse>> GetCampaignById(Guid id);
        Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignByUserId(string uid);
        Task<APIResponse<string>> DeleteCampaign(Guid CampaignId,DeleteCampaignRequest request);
    }
}
