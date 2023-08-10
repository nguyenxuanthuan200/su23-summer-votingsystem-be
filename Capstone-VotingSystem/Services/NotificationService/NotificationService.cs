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
            var checkAccout = await dbContext.Accounts.SingleOrDefaultAsync(p => p.UserName == request.Username && p.Status == true);
            if (checkAccout == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var id = Guid.NewGuid();
            Notification notification = new Notification();
            {
                notification.NotificationId = id;
                notification.Title = request.Title;
                notification.Message = request.Message;
                notification.CreateDate = currentDateTimeVn;
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

        public async Task<APIResponse<IEnumerable<NotificationResponse>>> GetNotificationId(string username)
        {
            APIResponse<IEnumerable<NotificationResponse>> response = new();
            var checkId = await dbContext.Accounts.Where(p => p.UserName == username && p.Status == true).SingleOrDefaultAsync();
            if (checkId == null)
            {
                response.ToFailedResponse("Tài khoản Không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkStatus = await dbContext.Notifications.Where(p => p.Status == true && p.Username == username).ToListAsync();
            if (checkStatus.Count() == 0)
            {
                response.ToFailedResponse("Không có thông báo nào", StatusCodes.Status400BadRequest);
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
            response.Data = result.OrderByDescending(c => c.CreateDate);
            response.ToSuccessResponse(response.Data, "Lấy danh sách thông báo thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<NotificationResponse>> UpdateNotification(Guid id)
        {
            APIResponse<NotificationResponse> response = new();
            var checkNoti = await dbContext.Notifications.SingleOrDefaultAsync(p => p.NotificationId == id && p.Status == true);
            if (checkNoti == null)
            {
                response.ToFailedResponse("Không tồn tại thông báo này", StatusCodes.Status400BadRequest);
                return response;
            }
            checkNoti.IsRead = true;
            dbContext.Update(checkNoti);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<NotificationResponse>(checkNoti);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
