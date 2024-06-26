﻿namespace Capstone_VotingSystem.Models.RequestModels.SearchRequest
{
    public class SearchCampaignRequest
    {
        public string? Keyword { get; set; }
        public string? Process { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
    }
}
