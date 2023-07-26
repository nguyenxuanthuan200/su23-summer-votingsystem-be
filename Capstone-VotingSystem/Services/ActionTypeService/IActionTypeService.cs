using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.TypeActionRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActionTypeResponse;

namespace Capstone_VotingSystem.Services.ActionTypeService
{
    public interface IActionTypeService
    {
        public Task<APIResponse<IEnumerable<ActionTypeResponse>>> GetAll();
        public Task<APIResponse<ActionTypeResponse>> CreateTypeAction(ActionTypeRequest request);
        public Task<APIResponse<ActionTypeResponse>> UpdateTypeAction(Guid? id, ActionTypeRequest request);
    }
}
