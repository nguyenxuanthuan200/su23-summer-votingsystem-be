using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.FeedbackRequest;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;
using Capstone_VotingSystem.Models.ResponseModels.FeedbackResponse;
using Microsoft.EntityFrameworkCore;
using Octokit.Internal;

namespace Capstone_VotingSystem.Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public FeedbackService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            this._mapper = mapper;
        }

        public async Task<APIResponse<string>> CheckFeedback(string userId, Guid campaignId)
        {
            APIResponse<string> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(p => p.UserId == userId && p.Status == true);
            if (checkUser == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCampaign = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == campaignId && p.Status == true);
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Không tìm thấy chiến dịch này hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            if (checkCampaign.Process != "Đang diễn ra")
            {
                response.ToFailedResponse("Không thể đánh giá chiến dịch khi nó chưa diễn ra hoặc đã kết thúc", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkFeedback = await dbContext.FeedBacks.SingleOrDefaultAsync(p => p.UserId == userId && p.Status == true && p.CampaignId == checkCampaign.CampaignId);
            if (checkFeedback != null)
            {
                response.ToFailedResponse("Đã đánh giá chiến dịch này rồi", StatusCodes.Status404NotFound);
                return response;
            }
            response.ToSuccessResponse("Chưa đánh giá chiến dịch này", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<FeedbackResponse>> CreateFeedback(FeedbackRequest request)
        {
            APIResponse<FeedbackResponse> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.Status == true);
            if (checkUser == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCampaign = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == request.CampaignId && p.Status == true && p.IsApprove == true);
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Không tìm thấy chiến dịch này hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            if (checkCampaign.Process != "Đang diễn ra")
            {
                response.ToFailedResponse("Không thể đánh giá chiến dịch khi nó chưa diễn ra hoặc đã kết thúc", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkFeedback = await dbContext.FeedBacks.SingleOrDefaultAsync(p => p.UserId == checkUser.UserId && p.Status == true && p.CampaignId == checkCampaign.CampaignId);
            if (checkFeedback == null)
            {
                response.ToFailedResponse("Đã đánh giá chiến dịch này rồi", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            FeedBack feedBack = new FeedBack();
            {
                feedBack.FeedBackId = id;
                feedBack.CampaignId = request.CampaignId;
                feedBack.UserId = request.UserId;
                feedBack.Status = true;
                feedBack.CreateDate = DateTime.Now;
                feedBack.Content = request.Content;
            }
            await dbContext.FeedBacks.AddAsync(feedBack);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<FeedbackResponse>(feedBack);
            response.ToSuccessResponse("Đánh giá thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteFeedback(Guid? feedbackid, DeleteFeedbackRequest request)
        {
            APIResponse<String> response = new();
            var user = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId && c.Status == true);
            if (user == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var cam = await dbContext.Campaigns.SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId && c.Status == true);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkFeedback = await dbContext.FeedBacks.SingleOrDefaultAsync(c => c.FeedBackId == feedbackid && c.Status == true);
            if (checkFeedback == null)
            {
                response.ToFailedResponse("Đánh giá không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkFeedback.UserId != user.UserId)
            {
                response.ToFailedResponse("Bạn không đủ quyền hạn để xóa đánh giá này ", StatusCodes.Status400BadRequest);
                return response;
            }
            checkFeedback.Status = false;
            dbContext.FeedBacks.Update(checkFeedback);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa đánh giá thành công", StatusCodes.Status200OK);
            return response;
        }

        //public async Task<APIResponse<IEnumerable<FeedbackResponse>>> GetAllFeedback()
        //{
        //    APIResponse<IEnumerable<FeedbackResponse>> response = new();
        //    var checkFeedbackId = await dbContext.FeedBacks.Where(p => p.Status == true).ToListAsync();
        //    if (checkFeedbackId == null)
        //    {
        //        response.ToFailedResponse("không có đánh giá", StatusCodes.Status404NotFound);
        //        return response;
        //    }
        //    IEnumerable<FeedbackResponse> result = checkFeedbackId.Select(
        //        x =>
        //        {
        //            return new FeedbackResponse()
        //            {
        //                FeedBackId = x.FeedBackId,
        //                Status = x.Status,
        //                CampaignId = x.CampaignId,
        //                Content = x.Content,
        //                CreateDate = x.CreateDate,
        //                UserId = x.UserId
        //            };
        //        }
        //        ).ToList();
        //    if (result == null)
        //    {
        //        response.ToFailedResponse("lấy thông tin thất bại", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    response.ToSuccessResponse(response.Data = result, "lấy thông tin thành công", StatusCodes.Status200OK);
        //    return response;
        //}

        public async Task<APIResponse<IEnumerable<FeedbackResponse>>> GetFeedBackByCampaignId(Guid campaignid)
        {
            APIResponse<IEnumerable<FeedbackResponse>> response = new();
            var checkFeedback = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == campaignid);
            if (checkFeedback == null)
            {
                response.ToFailedResponse("Không tìm thấy đánh giá nào", StatusCodes.Status404NotFound);
                return response;
            }
            var feedback = await dbContext.FeedBacks.Where(p => p.CampaignId == campaignid).ToListAsync();
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

        public async Task<APIResponse<IEnumerable<FeedbackResponse>>> GetFeedbackByUserId(string userId)
        {
            APIResponse<IEnumerable<FeedbackResponse>> response = new();
            var user = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == userId && c.Status == true);
            if (user == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            var feedback = await dbContext.FeedBacks.Where(p => p.UserId == userId).ToListAsync();
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
            if (result.Count() == 0)
            {
                response.ToFailedResponse("Danh sách trống", StatusCodes.Status400BadRequest);
                return response;
            }
            foreach (var item in result)
            {
                var getCampaign = await dbContext.Campaigns.Where(p => p.CampaignId == item.CampaignId).SingleOrDefaultAsync();
                item.CampaignName = getCampaign.Title;

            }
            response.ToSuccessResponse(result, "lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
    }

}

