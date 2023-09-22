using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Core.CoreModel;
namespace Capstone_VotingSystem.Services.CandidateService
{
    public interface ICandidateService
    {
        Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetListCandidateCampaign(Guid campaignId);
        //Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetAllCandidate();
        Task<APIResponse<GetCandidateDetailResponse>> GetCandidateByStageId(Guid candidateId, Guid stageId);
        Task<APIResponse<GetCandidateByIdResponse>> GetCandidateById(Guid candidateId);
        Task<APIResponse<string>> CreateCandidateCampaign(CreateCandidateCampaignRequest request);
        Task<APIResponse<string>> CreateListCandidate(CreateListCandidateRequest request);
        Task<APIResponse<string>> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request);
        Task<APIResponse<string>> DeleteCandidateCampaign(Guid candidateId, DeleteCandidateRequest request);
        Task<APIResponse<GetListCandidateByUserIdResponse>> UpdateCandidateProfile(Guid id, UpdateCandidateProfileRequest request);
        Task<APIResponse<GetListCandidateStageResponse>> GetListCandidatStage(Guid stageId, string userId);
        Task<APIResponse<IEnumerable<GetListCandidateByUserIdResponse>>> GetListCandidateByUserId(string userId);
    }
}
