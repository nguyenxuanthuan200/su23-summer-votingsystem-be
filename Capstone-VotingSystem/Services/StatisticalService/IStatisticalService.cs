using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.StatisticalResponse;

namespace Capstone_VotingSystem.Services.StatisticalService
{
    public interface IStatisticalService
    {
        Task<APIResponse<IEnumerable<GetResultCampaignResponse>>> GetResultCampaign(Guid campaignId);
        Task<APIResponse<StatisticalTotalResponse>> StatisticalTotal();
    }
}
