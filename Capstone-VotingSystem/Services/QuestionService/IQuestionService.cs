using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.ElementRequest;
using Capstone_VotingSystem.Models.RequestModels.QuestionRequest;
using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;

namespace Capstone_VotingSystem.Services.QuestionService
{
    public interface IQuestionService
    {
        Task<APIResponse<IEnumerable<GetQuestionResponse>>> GetListQuestionForm(Guid formid);
        Task<APIResponse<GetQuestionResponse>> GetQuestionById(Guid questionId);
        Task<APIResponse<int>> GetNumberQuestionInForm(Guid formid);
        Task<APIResponse<string>> CreateQuestion(CreateListQuestionRequest request);
        Task<APIResponse<GetQuestionNoElementResponse>> CreateQuestionNoElement(CreateQuestionWithNoElementRequest request);
        Task<APIResponse<GetQuestionResponse>> CreateElementQuestion(Guid questionId,CreateElementRequest request);
        Task<APIResponse<GetQuestionResponse>> UpdateQuestion(Guid id,UpdateQuestionRequest request);
        Task<APIResponse<string>> DeleteQuestion(Guid id);
    }
}
