﻿namespace Capstone_VotingSystem.Models.ResponseModels.ScoreResponse
{
    public class GetStageScoreResponse
    {
        public Guid StageId { get; set; }
        public string StageName { get; set; }
        public double StageScore { get; set; }
    }
}
