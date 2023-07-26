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
        public async Task<APIResponse<GetActivityResponse>> CreateActivity(CreateActivityRequest request)
        {
            APIResponse<GetActivityResponse> response = new();
            var checkUser = await dbContext.Candidates.SingleOrDefaultAsync(c => c.CandidateId == request.CandidateId);
            if (checkUser == null)
            {
                response.ToFailedResponse("Candidate không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }

            var id = Guid.NewGuid();
            Activity acti = new Activity();
            {
                acti.ActivityId = id;
                acti.Title = request.Title;
                acti.Content = request.Content;
                acti.CandidateId = request.CandidateId;
            };
            await dbContext.Activities.AddAsync(acti);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetActivityResponse>(acti);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteActivity(Guid activityId, DeleteActivityRequest request)
        {
            APIResponse<string> response = new();
            var acti = await dbContext.Activities.SingleOrDefaultAsync(c => c.ActivityId == activityId);
            if (acti == null)
            {
                response.ToFailedResponse("Activity không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            var checkus = await dbContext.Candidates.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CandidateId == request.CandidateId);
            if (checkus == null)
            {
                response.ToFailedResponse("Candidate không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            dbContext.Activities.Remove(acti);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa Activity thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetActivityResponse>>> GetActivityByCandidate(Guid candidateId)
        {
            APIResponse<IEnumerable<GetActivityResponse>> response = new();
            var checkUser = await dbContext.Candidates.Where(p => p.CandidateId == candidateId && p.Status == true)
               .SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Candidate không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var getById = await dbContext.Activities.Where(p => p.CandidateId == candidateId)
                .ToListAsync();
            if (getById == null || getById.Count==0)
            {
                response.ToFailedResponse(" Không có Activity nào tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetActivityResponse> result = getById.Select(
               x =>
               {
                   return new GetActivityResponse()
                   {
                       ActivityId = x.ActivityId,
                       Title = x.Title,
                       Content = x.Content,
                       CandidateId = x.CandidateId,
                   };
               }
               ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetActivityResponse>> UpdateActivity(Guid id, UpdateActivityRequest request)
        {
            APIResponse<GetActivityResponse> response = new();
            var cam = await dbContext.Activities.SingleOrDefaultAsync(c => c.ActivityId == id);
            if (cam == null)
            {
                response.ToFailedResponse("Activity không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.Title = request.Title;
            cam.Content = request.Content;
            dbContext.Activities.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetActivityResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
