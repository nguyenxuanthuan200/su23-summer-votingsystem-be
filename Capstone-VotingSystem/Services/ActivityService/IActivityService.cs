using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.ActivityRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActivityResponse;

namespace Capstone_VotingSystem.Services.ActivityService
{
    public interface IActivityService
    {
        Task<APIResponse<IEnumerable<GetActivityResponse>>> GetActivityByCandidate(Guid candidateId);
        Task<APIResponse<GetActivityResponse>> CreateActivity(CreateActivityRequest request);
        Task<APIResponse<GetActivityResponse>> UpdateActivity(Guid id, UpdateActivityRequest request);
        Task<APIResponse<string>> DeleteActivity(Guid activityId, DeleteActivityRequest request);
    }
}
