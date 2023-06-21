using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.Group;
using Capstone_VotingSystem.Models.ResponseModels.GroupResponse;

namespace Capstone_VotingSystem.Services.GroupService
{
    public interface IGroupService
    {
        Task<APIResponse<IEnumerable<GroupResponse>>> GetListGroup();
        Task<APIResponse<GroupResponse>> CreateGroup(CreateGroupRequest request);
        Task<APIResponse<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request);
    }
}
