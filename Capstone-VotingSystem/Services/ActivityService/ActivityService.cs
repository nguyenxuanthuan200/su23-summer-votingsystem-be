using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.ActivityRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActivityResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.ActivityService
{
    public class ActivityService : IActivityService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public ActivityService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<APIResponse<string>> CreateActivityContent(CreateActivityRequest request)
        {
            APIResponse<string> response = new();
            var checkCandidate = await dbContext.Candidates.SingleOrDefaultAsync(c => c.CandidateId == request.CandidateId && c.Status == true);
            if (checkCandidate == null)
            {
                response.ToFailedResponse("Ứng cử viên không tồn tại hoặc đã bị loại khỏi chiến dịch", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkAccount = await dbContext.Users.Where(p => p.UserId == checkCandidate.UserId).SingleOrDefaultAsync();
            if (checkAccount == null || checkAccount.Status == false)
            {
                response.ToFailedResponse("Tài khoản của ứng cử viên không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }

            var checkActivity = await dbContext.Activities.Where(p => p.ActivityId == request.ActivityId).SingleOrDefaultAsync();
            if (checkActivity == null)
            {
                response.ToFailedResponse("Hoạt động không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            ActivityContent acti = new ActivityContent();
            {
                acti.ActivityContentId = id;
                acti.Content = request.Content;
                acti.ActivityId = request.ActivityId;
                acti.CandidateId = request.CandidateId;
            };
            await dbContext.ActivityContents.AddAsync(acti);
            await dbContext.SaveChangesAsync();
            //var map = _mapper.Map<GetActivityResponse>(acti);
            response.ToSuccessResponse("Tạo nội dung hoạt động thành công", StatusCodes.Status200OK);
            //response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteActivityContent(Guid activityId)
        {
            APIResponse<string> response = new();
            var acti = await dbContext.ActivityContents.SingleOrDefaultAsync(c => c.ActivityContentId == activityId);
            if (acti == null)
            {
                response.ToFailedResponse("Hoạt động không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            dbContext.ActivityContents.Remove(acti);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa nội dung hoạt động thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetActivityResponse>>> GetActivity()
        {
            APIResponse<IEnumerable<GetActivityResponse>> response = new();

            var getListActivity = await dbContext.Activities.ToListAsync();
            if (getListActivity.Count <= 0)
            {
                response.ToFailedResponse(" Không có hoạt động nào tồn tại trong hệ thống hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetActivityResponse> result = getListActivity.Select(
               x =>
               {
                   return new GetActivityResponse()
                   {
                       ActivityId = x.ActivityId,
                       Title = x.Title,
                       Content = x.Content,
                   };
               }
               ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetActivityByCandidateResponse>>> GetActivityByCandidateId(Guid candidateId)
        {
            APIResponse<IEnumerable<GetActivityByCandidateResponse>> response = new();
            var listActivity = new List<GetActivityByCandidateResponse>();
            var activity = new GetActivityByCandidateResponse();

            var listContent = new List<GetActivityContentResponse>();
            var content = new GetActivityContentResponse();

            var checkCandidate = await dbContext.Candidates.Where(p => p.CandidateId == candidateId && p.Status == true).SingleOrDefaultAsync();
            if (checkCandidate == null)
            {
                response.ToFailedResponse("Ứng cử viên không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkAccount = await dbContext.Users.Where(p => p.UserId == checkCandidate.UserId).SingleOrDefaultAsync();
            if (checkAccount == null || checkAccount.Status == false)
            {
                response.ToFailedResponse("Ứng cử viên không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }

            var getActivityContent = await dbContext.ActivityContents.Where(p => p.CandidateId == candidateId)
                .ToListAsync();
            if (getActivityContent == null || getActivityContent.Count <= 0)
            {
                response.ToFailedResponse(" Không có hoạt động nào tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }

            var getActivity = await dbContext.Activities.ToListAsync();
            foreach (var i in getActivity)
            {
                activity = new();
                listContent = new();
                foreach (var item in getActivityContent)
                {

                    if (i.ActivityId == item.ActivityId)
                    {
                        content = new();
                        content.ActivityContentId = item.ActivityContentId;
                        content.Content = item.Content;

                        listContent.Add(content);
                    }
                }
                if (listContent.Count>0)
                {
                    activity.ListContent = listContent;
                    activity.Title = i.Title;
                    activity.ActivityId = i.ActivityId;
                    listActivity.Add(activity);
                }

            }
            response.Data = listActivity;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<string>> UpdateActivityContent(Guid id, UpdateActivityRequest request)
        {
            APIResponse<string> response = new();
            var content = await dbContext.ActivityContents.SingleOrDefaultAsync(c => c.ActivityContentId == id);
            if (content == null)
            {
                response.ToFailedResponse("Không thể sửa nội dung không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            content.Content = request.Content;
            dbContext.ActivityContents.Update(content);
            await dbContext.SaveChangesAsync();
            //var map = _mapper.Map<GetActivityResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            //response.Data = map;
            return response;
        }
    }
}
