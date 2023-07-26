using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.SearchRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Models.ResponseModels.SearchReponse;

namespace Capstone_VotingSystem.Services.SearchService
{
    public interface ISearchService
    {
        Task<APIResponse<PagedListCandidateResponse>> SearchFilterCandidate(SearchCandidateRequest request);
        Task<APIResponse<PagedListCampaignResponse>> SearchFilterCampaign(SearchCampaignRequest request);
    }
}
