﻿namespace Capstone_VotingSystem.Models.ResponseModels.FormResponse
{
    public class GetListQuestionFormResponse
    {
        public Guid FormId { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
        public string? UserId { get; set; }
        public string? Visibility { get; set; }

        public List<GetListQuestionResponse?> Questions { get; set; }
    }
}
