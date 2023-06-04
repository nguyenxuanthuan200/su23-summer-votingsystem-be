using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;

namespace Capstone_VotingSystem.Repositories.CandidateRepo
{
    public interface ICandidateRepositories
    {
        Task<IEnumerable<GetListCandidateCampaignResponse>> GetListCandidateCampaign(Guid campaignId);
        Task<GetCandidateCampaignResponse> CreateCandidateCampaign(CreateCandidateCampaignRequest request);
        Task<CreateAccountCandidateResponse> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request);
        Task<Boolean> DeleteCandidateCampaign(Guid id);
        Task<UpdateCandidateProfileResponse> UpdateCandidateProfile(Guid id, UpdateCandidateProfileRequesst request);
    }
}
