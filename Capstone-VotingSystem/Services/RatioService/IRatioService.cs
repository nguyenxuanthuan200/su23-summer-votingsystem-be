using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.RatioRequest;
using Capstone_VotingSystem.Models.ResponseModels.RatioResponse;

namespace Capstone_VotingSystem.Services.RatioService
{
    public interface IRatioService
    {
        Task<APIResponse<IEnumerable<RatioResponse>>> GetRatioByCampaign(Guid campaignId);
        Task<APIResponse<RatioResponse>> CreateCampaignRatio(CreateRatioRequest request);
        Task<APIResponse<RatioResponse>> UpdateRatio(Guid id, UpdateRatioRequest request);
    }
}
