﻿using Capstone_VotingSystem.Models.RequestModels.ElementRequest;

namespace Capstone_VotingSystem.Models.RequestModels.QuestionRequest
{
    public class CreateQuestionRequest
    {
        public string Content { get; set; }
        public Guid TypeId { get; set; }
        public int Score { get; set; }
        public List<CreateElementRequest> Element { get; set; }
    }
}
