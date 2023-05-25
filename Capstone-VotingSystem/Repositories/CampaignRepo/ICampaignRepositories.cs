using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
namespace Capstone_VotingSystem.Repositories.CampaignRepo
{
    public interface ICampaignRepositories
    {
        Task<IEnumerable<GetCampaignResponse>> GetCampaign();
    }
}
