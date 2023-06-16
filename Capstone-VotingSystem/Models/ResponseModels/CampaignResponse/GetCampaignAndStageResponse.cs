﻿using Capstone_VotingSystem.Models.ResponseModels.StageResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class GetCampaignAndStageResponse
    {
        public Guid CampaignId { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Visibility { get; set; }
        public string? ImgUrl { get; set; }
        //public bool? Status { get; set; }
        public string? UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public List<GetStageResponse> Stage { get; set; }
    }
}
