using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;
using Capstone_VotingSystem.Models.ResponseModels.FeedbackResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        public FeedbackService(VotingSystemContext votingSystemContext)
        {
            this.dbContext = votingSystemContext;
        }

        public VotingSystemContext dbContext { get; }

        public async Task<APIResponse<IEnumerable<FeedbackResponse>>> GetAllFeedback()
        {
            APIResponse<IEnumerable<FeedbackResponse>> response = new();
            var checkFeedbackId = await dbContext.FeedBacks.Where(p => p.Status == true).ToListAsync();
            if (checkFeedbackId == null)
            {
                response.ToFailedResponse("không có đánh giá", StatusCodes.Status404NotFound);
                return response;
            }
            IEnumerable<FeedbackResponse> result = checkFeedbackId.Select(
                x =>
                {
                    return new FeedbackResponse()
                    {
                        FeedBackId = x.FeedBackId,
                        Status = x.Status,
                        CampaignId = x.CampaignId,
                        Content = x.Content,
                        CreateDate = x.CreateDate,
                        UserId = x.UserId
                    };
                }
                ).ToList();
            if (result == null)
            {
                response.ToFailedResponse("lấy thông tin thất bại", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "lấy thông tin thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<FeedbackResponse>>> GetByFeedBackId(Guid? feedbackid)
        {
            APIResponse<IEnumerable<FeedbackResponse>> response = new();
            var checkFeedback = await dbContext.FeedBacks.SingleOrDefaultAsync(p => p.FeedBackId == feedbackid);
            if (checkFeedback == null)
            {
                response.ToFailedResponse("không tìm thấy feedback", StatusCodes.Status404NotFound);
                return response;
            }
            var feedback = await dbContext.FeedBacks.Where(p => p.FeedBackId == feedbackid).ToListAsync();
            IEnumerable<FeedbackResponse> result = feedback.Select(
                x =>
                {
                    return new FeedbackResponse()
                    {
                        FeedBackId = x.FeedBackId,
                        CampaignId = x.CampaignId,
                        Content = x.Content,
                        CreateDate = x.CreateDate,
                        Status = x.Status,
                        UserId = x.UserId
                    };
                }
                ).ToList();
            if (result == null)
            {
                response.ToFailedResponse("lấy danh sách thất bại", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(result, "lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
    }

}

