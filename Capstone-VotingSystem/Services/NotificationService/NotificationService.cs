using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.NotificationRequest;
using Capstone_VotingSystem.Models.ResponseModels.CategoryResponse;
using Capstone_VotingSystem.Models.ResponseModels.FeedbackResponse;
using Capstone_VotingSystem.Models.ResponseModels.NotificationResponse;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Capstone_VotingSystem.Services.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public NotificationService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            this._mapper = mapper;
        }

        public async Task<APIResponse<NotificationResponse>> CreateNotification(NotificationRequest request)
        {
            APIResponse<NotificationResponse> response = new();
            var checkAccout = await dbContext.Accounts.SingleOrDefaultAsync(p => p.UserName == request.Username);
            if (checkAccout == null)
            {
                response.ToFailedResponse("không tồn tại user", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Notification notification = new Notification();
            {
                notification.NotificationId = id;
                notification.Title = request.Title;
                notification.Message = request.Message;
                notification.CreateDate = DateTime.UtcNow;
                notification.IsRead = false;
                notification.Status = true;
                notification.Username = request.Username;
            }
            await dbContext.AddRangeAsync(notification);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<NotificationResponse>(notification);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;


        }

        public async Task<APIResponse<IEnumerable<NotificationResponse>>> GetNotificationId(string? username)
        {
            APIResponse<IEnumerable<NotificationResponse>> response = new();
            var checkId = await dbContext.Accounts.Where(p => p.UserName == username).SingleOrDefaultAsync();
            if (checkId == null)
            {
                response.ToFailedResponse("tài khoản Không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkStatus = await dbContext.Notifications.Where(p => p.Status == true).ToListAsync();
            if (checkStatus == null)
            {
                response.ToFailedResponse("Không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<NotificationResponse> result = checkStatus.Select(
                 x =>
                 {
                     return new NotificationResponse()
                     {
                         NotificationId = x.NotificationId,
                         Title = x.Title,
                         Message = x.Message,
                         CreateDate = x.CreateDate,
                         IsRead = x.IsRead,
                         Status = x.Status,
                         Username = x.Username,
                     };
                 }
                 ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<NotificationResponse>> UpdateNotification(Guid? id, NotificationRequest request)
        {
            APIResponse<NotificationResponse> response = new();
            var checkAccout = await dbContext.Notifications.SingleOrDefaultAsync(p => p.NotificationId == id);
            if (checkAccout == null)
            {
                response.ToFailedResponse("không tồn tại user", StatusCodes.Status400BadRequest);
                return response;
            }
            checkAccout.Title = request.Title;
            checkAccout.Message = request.Message;
            checkAccout.CreateDate = DateTime.UtcNow;
            checkAccout.IsRead = true;
            checkAccout.Status = true;

            dbContext.Update(checkAccout);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<NotificationResponse>(checkAccout);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
