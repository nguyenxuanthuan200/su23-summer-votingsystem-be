using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.TypeRequest;
using Capstone_VotingSystem.Models.ResponseModels.TypeResponse;

namespace Capstone_VotingSystem.Services.TypeService
{
    public interface ITypeService
    {
        Task<APIResponse<IEnumerable<TypeResponse>>> GetListType();
        Task<APIResponse<TypeResponse>> CreateType(CreateTypeRequest request);
        Task<APIResponse<TypeResponse>> UpdateType(Guid id,UpdateTypeRequest request);
    }
}
