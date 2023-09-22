using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.FeedbackRequest;
using Capstone_VotingSystem.Models.ResponseModels.FeedbackResponse;

namespace Capstone_VotingSystem.Services.FeedbackService
{
    public interface IFeedbackService
    {
        //public Task<APIResponse<IEnumerable<FeedbackResponse>>> GetAllFeedback();
        public Task<APIResponse<string>> CheckFeedback(string userId,Guid campaignId);
        public Task<APIResponse<IEnumerable<FeedbackResponse>>> GetFeedbackByUserId(string userId);
        public Task<APIResponse<IEnumerable<FeedbackResponse>>> GetFeedBackByCampaignId(Guid campaignid);
        public Task<APIResponse<FeedbackResponse>> CreateFeedback(FeedbackRequest request);
        public Task<APIResponse<string>> DeleteFeedback(Guid? feedbackid, DeleteFeedbackRequest request);
    }

}

