using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.GroupRequest;
using Capstone_VotingSystem.Models.ResponseModels.GroupResponse;

namespace Capstone_VotingSystem.Services.GroupService
{
    public interface IGroupService
    {
        Task<APIResponse<IEnumerable<GroupResponse>>> GetListGroup();
        Task<APIResponse<IEnumerable<GroupResponse>>> GetListGroupByCampaign(Guid campaignId);
        Task<APIResponse<StatisticalGroupResponse>> StatisticalGroupByCampaign(Guid campaignId);
        Task<APIResponse<GroupResponse>> CreateGroup(CreateGroupRequest request);
        Task<APIResponse<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request);
        Task<APIResponse<string>> CheckGroupUser(string userName,Guid campaignID);
        //Task<APIResponse<string>> DeleteGroup(Guid groupId);
    }
}
