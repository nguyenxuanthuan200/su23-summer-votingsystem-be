﻿namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class CampaignResponse
    {
        public Guid CampaignId { get; set; }
        public string? Title { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? Visibility { get; set; }
        public bool? Status { get; set; }
        public string? Username { get; set; }
    }
}
