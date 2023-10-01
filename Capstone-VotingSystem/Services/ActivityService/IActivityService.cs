using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.ActivityRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActivityResponse;

namespace Capstone_VotingSystem.Services.ActivityService
{
    public interface IActivityService
    {
        Task<APIResponse<IEnumerable<GetActivityByCandidateResponse>>> GetActivityByCandidateId(Guid candidateId);
        Task<APIResponse<IEnumerable<GetActivityResponse>>> GetActivity();
        Task<APIResponse<string>> CreateActivityContent(CreateActivityRequest request);
        Task<APIResponse<string>> UpdateActivityContent(Guid id, UpdateActivityRequest request);
        Task<APIResponse<string>> DeleteActivityContent(Guid activityContentId);
    }
}
