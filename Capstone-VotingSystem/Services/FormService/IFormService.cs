using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.FormRequest;
using Capstone_VotingSystem.Models.ResponseModels.FormResponse;

namespace Capstone_VotingSystem.Services.FormService
{
    public interface IFormService
    {
        Task<APIResponse<IEnumerable<GetFormResponse>>> GetAllForm();
        Task<APIResponse<GetFormResponse>> CreateForm(CreateFormRequest request);
        Task<APIResponse<GetFormResponse>> UpdateForm(Guid id,UpdateFormByUser request);
        Task<APIResponse<string>> DeleteForm(DeleteFormRequest request);
    }
}
