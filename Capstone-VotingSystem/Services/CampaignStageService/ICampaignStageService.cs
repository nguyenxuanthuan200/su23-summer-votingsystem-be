﻿using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse;

namespace Capstone_VotingSystem.Repositories.CampaignStageRepo
{
    public interface ICampaignStageService
    {
        Task<IEnumerable<GetCampaignStageByCampaignResponse>> GetCampaignStageByCampaign(Guid campaignId);
        Task<CreateCampaginStageResponse> CreateCampaignStage(CreateCampaignStageRequest request);
        Task<GetCampaignStageByCampaignResponse> UpdateCampaignStage(UpdateCampaignStageRequest request);
    }
}
