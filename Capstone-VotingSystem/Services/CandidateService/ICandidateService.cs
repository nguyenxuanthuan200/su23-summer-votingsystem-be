    using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Core.CoreModel;
namespace Capstone_VotingSystem.Services.CandidateService
{
    public interface ICandidateService
    {
        Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetListCandidateCampaign(Guid campaignId);
        Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetAllCandidate();
        Task<APIResponse<CreateCandidateCampaignResponse>> CreateCandidateCampaign(CreateCandidateCampaignRequest request);
        Task<APIResponse<CreateAccountCandidateResponse>> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request);
        Task<APIResponse<string>> DeleteCandidateCampaign(Guid candidateId,Guid campaignId,string userId);
        //Task<UpdateCandidateProfileResponse> UpdateCandidateProfile(Guid id, UpdateCandidateProfileRequesst request);
    }
}
