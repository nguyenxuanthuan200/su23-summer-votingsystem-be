﻿using Capstone_VotingSystem.Models.ResponseModels.ActivityResponse;

namespace Capstone_VotingSystem.Models.ResponseModels.CandidateResponse
{
    public class GetCandidateDetailResponse
    {
        public Guid CandidateId { get; set; }
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public Guid StageId { get; set; }
        public Guid CampaignId { get; set; }
        public Guid? FormId { get; set; }
        public string GroupName { get; set; }
        public int Score { get; set; }
        public List<GetActivityByCandidateResponse> ListActivity { get; set; }
    }
}
