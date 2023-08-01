using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.NotificationRequest;
using Capstone_VotingSystem.Models.ResponseModels.NotificationResponse;

namespace Capstone_VotingSystem.Services.NotificationService
{
    public interface INotificationService
    {
        public Task<APIResponse<IEnumerable<NotificationResponse>>> GetNotificationId(string? username);
        public Task<APIResponse<NotificationResponse>> CreateNotification(NotificationRequest request);
        public Task<APIResponse<NotificationResponse>> UpdateNotification(Guid? id);
    }
}
