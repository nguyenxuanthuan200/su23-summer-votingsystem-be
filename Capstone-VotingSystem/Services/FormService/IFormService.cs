using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.FormRequest;
using Capstone_VotingSystem.Models.ResponseModels.FormResponse;

namespace Capstone_VotingSystem.Services.FormService
{
    public interface IFormService
    {
        Task<APIResponse<IEnumerable<GetFormResponse>>> GetAllForm();
        Task<APIResponse<GetListQuestionFormResponse>> GetFormById(Guid formId);
        Task<APIResponse<IEnumerable<GetFormResponse>>> GetFormByUserId(string id);
        Task<APIResponse<GetFormResponse>> CreateForm(CreateFormRequest request);
        Task<APIResponse<GetFormResponse>> UpdateForm(Guid id, UpdateFormByUser request);
        Task<APIResponse<string>> DeleteForm(Guid formId, DeleteFormRequest request);
        Task<APIResponse<IEnumerable<GetFormResponse>>> GetFormNeedApprove();
        Task<APIResponse<string>> ApproveForm(Guid id);
    }
}
