using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
namespace Capstone_VotingSystem.Models.ResponseModels.SearchReponse
{
    public class PagedListCampaignResponse
    {
        public int? Total { get; set; }
        public IEnumerable<GetCampaignAndStageResponse> Campaign { get; set; }
    }
}
