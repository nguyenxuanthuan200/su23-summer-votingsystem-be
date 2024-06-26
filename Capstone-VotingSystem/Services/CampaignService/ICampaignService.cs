﻿using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;

namespace Capstone_VotingSystem.Services.CampaignService
{
    public interface ICampaignService
    {
        Task<APIResponse<IEnumerable<GetCampaignAndStageResponse>>> GetCampaign();
        Task<APIResponse<GetCampaignResponse>> UpdateCampaign(Guid id, UpdateCampaignRequest request);
        Task<APIResponse<GetCampaignResponse>> CreateCampaign(CreateCampaignRequest request);
        Task<APIResponse<GetCampaignAndStageResponse>> GetCampaignById(Guid id);
        Task<APIResponse<GetCampaignAndStageResponse>> GetCampaignRepresentative();
        Task<APIResponse<string>> UpdateProcess();
        Task<APIResponse<string>> UpdatePublicResult(Guid campaignId);
        Task<APIResponse<string>> UpdateCampaignRepresentative(Guid id);
        Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignByUserId(string uid);
        Task<APIResponse<IEnumerable<GetCampaignForAdminResponse>>> GetCampaignForAdmin();
        //Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignNeedApprove();
        Task<APIResponse<GetCampaignResponse>> ApproveCampaign(Guid id);
        Task<APIResponse<string>> DeleteCampaign(Guid CampaignId, DeleteCampaignRequest request);
        Task<APIResponse<string>> UnDeleteCampaign(Guid CampaignId);
        Task<APIResponse<ImageCampaignResponse>> AddImageCampaignAsync(IFormFile file, string folderName, Guid? campaignId);
    }
}
