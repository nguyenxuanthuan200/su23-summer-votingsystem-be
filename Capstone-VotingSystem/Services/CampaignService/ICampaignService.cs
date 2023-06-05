using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
namespace Capstone_VotingSystem.Repositories.CampaignRepo
{
    public interface ICampaignService
    {
        Task<IEnumerable<GetCampaignResponse>> GetCampaign();
        //Task<IEnumerable<GetCampaignResponse>> GetCampaignByType(Guid id);
        //Task<IEnumerable<GetCampaignResponse>> GetCampaignByCampus(Guid id);
        Task<GetCampaignResponse> UpdateCampaign(UpdateCampaignRequest request);
        Task<GetCampaignResponse> CreateCampaign(CreateCampaignRequest request);
        Task<GetCampaignResponse> GetCampaignById(Guid id);
    }
}
