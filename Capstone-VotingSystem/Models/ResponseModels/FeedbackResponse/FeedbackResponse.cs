﻿namespace Capstone_VotingSystem.Models.ResponseModels.FeedbackResponse
{
    public class FeedbackResponse
    {
        public Guid FeedBackId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? Status { get; set; }
        public string? UserId { get; set; }
        public Guid? CampaignId { get; set; }
        public string? CampaignName { get; set; }
    }
}
