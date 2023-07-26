using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.SearchReponse
{
    public class PagedListCandidateResponse
    {
        public int? Total { get; set; }
        public IEnumerable<GetListCandidateCampaignResponse> Candidate { get; set; }
    }
}
