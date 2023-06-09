﻿using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.ElementRequest;
using Capstone_VotingSystem.Models.RequestModels.QuestionRequest;
using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;

namespace Capstone_VotingSystem.Services.QuestionService
{
    public interface IQuestionService
    {
        Task<APIResponse<IEnumerable<GetQuestionResponse>>> GetListQuestionForm(Guid formid);
        Task<APIResponse<GetQuestionResponse>> CreateQuestion(CreateQuestionRequest request);
        Task<APIResponse<GetQuestionResponse>> CreateElementQuestion(Guid questionId,CreateElementRequest request);
        Task<APIResponse<GetQuestionResponse>> UpdateQuestion(Guid id,UpdateQuestionRequest request);
    }
}
