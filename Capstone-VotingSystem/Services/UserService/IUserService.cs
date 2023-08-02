using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.ActivityRequest;
using Capstone_VotingSystem.Models.RequestModels.UserRequest;
using Capstone_VotingSystem.Models.ResponseModels.UserResponse;

namespace Capstone_VotingSystem.Services.UserService
{
    public interface IUserService
    {
        public Task<APIResponse<IEnumerable<GetListUserResponse>>> GetAllUser();
        public Task<APIResponse<UpdateUserResponse>> UpdateUser(string? userId, UpdateUserRequest request);
        public Task<APIResponse<string>> UpdateUserGroup(string userId, Guid groupId, Guid campaignId);
        public Task<APIResponse<GetListUserResponse>> UpdateUserPermission(string userId, UserPermissionRequest request);
    }
}
