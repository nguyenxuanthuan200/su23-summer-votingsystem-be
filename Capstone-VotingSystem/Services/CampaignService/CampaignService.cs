using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;

namespace Capstone_VotingSystem.Services.CampaignService
{
    public class CampaignService : ICampaignService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public CampaignService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<GetCampaignResponse>> CreateCampaign(CreateCampaignRequest request)
        {
            APIResponse<GetCampaignResponse> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(c => c.UserName == request.UserName);
            if (checkUser == null)
            {
                response.ToFailedResponse("UserName không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Campaign cam = new Campaign();
            {
                cam.CampaignId = id;
                cam.StartTime = request.StartTime;
                cam.EndTime = request.EndTime;
                cam.Status = request.Status;
                cam.Title = request.Title;
                cam.Visibility = request.Visibility;
                cam.UserName = request.UserName;
            };
            await dbContext.Campaigns.AddAsync(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<GetCampaignResponse>> DeleteCampaign(DeleteCampaignRequest request)
        {
            APIResponse<GetCampaignResponse> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId);

            if (cam == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkus = await dbContext.Accounts.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.UserName == request.UserName);
            if (checkus == null)
            {
                response.ToFailedResponse("Account không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.Status = false;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Xóa Campaign thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaign()
        {
            APIResponse<IEnumerable<GetCampaignResponse>> response = new();
            var campaign = await dbContext.Campaigns.ToListAsync();
            if (campaign == null)
            {
                response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetCampaignResponse> result = campaign.Select(
                x =>
                {
                    return new GetCampaignResponse()
                    {
                        CampaignId = x.CampaignId,
                        StartTime = x.StartTime,
                        EndTime = x.EndTime,
                        Status = x.Status,
                        Title = x.Title,
                        Visibility = x.Visibility,
                        UserName = x.UserName,
                    };
                }
                ).ToList();
            if (result == null)
            {
                response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
        public async Task<APIResponse<GetCampaignResponse>> GetCampaignById(Guid id)
        {
            APIResponse<GetCampaignResponse> response = new();
            var getById = await dbContext.Campaigns.Where(p => p.CampaignId == id && p.Status == true)
                .SingleOrDefaultAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var map = _mapper.Map<GetCampaignResponse>(getById);
            response.ToSuccessResponse("Lấy thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
        public async Task<APIResponse<GetCampaignResponse>> UpdateCampaign(Guid id, UpdateCampaignRequest request)
        {
            APIResponse<GetCampaignResponse> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == id);
            if (cam == null)
            {
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.StartTime = request.StartTime;
            cam.EndTime = request.EndTime;
            cam.Status = request.Status;
            cam.Visibility = request.Visibility;
            cam.Title = request.Title;
            //cam.CampusId = request.CampusId;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
