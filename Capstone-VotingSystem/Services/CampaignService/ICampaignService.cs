using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;

namespace Capstone_VotingSystem.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaign();
        Task<APIResponse<GetCampaignResponse>> UpdateCampaign(Guid id,UpdateCampaignRequest request);
        Task<APIResponse<GetCampaignResponse>> CreateCampaign(CreateCampaignRequest request);
        Task<APIResponse<GetCampaignResponse>> GetCampaignById(Guid id);
        Task<APIResponse<GetCampaignResponse>> DeleteCampaign(DeleteCampaignRequest request);
    }
}
