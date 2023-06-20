using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.StageResponse;

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
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId);
            if (checkUser == null)
            {
                response.ToFailedResponse("UserName không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            //var checkCategory = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            //if (checkCategory == null)
            //{
            //    response.ToFailedResponse("Category không tồn tại", StatusCodes.Status400BadRequest);
            //    return
            //    response;
            //}
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Campaign cam = new Campaign();
            {
                cam.CampaignId = id;
                cam.StartTime = request.StartTime;
                cam.EndTime = request.EndTime;
                cam.Visibility = request.Visibility;
                cam.Status = true;
                cam.Title = request.Title;
                cam.CategoryId = request.CategoryId;
                cam.UserId = request.UserId;
            };
            await dbContext.Campaigns.AddAsync(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteCampaign(Guid CampaignId, DeleteCampaignRequest request)
        {
            APIResponse<string> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == CampaignId);
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
            //var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Xóa Campaign thành công", StatusCodes.Status200OK);
            //response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignAndStageResponse>>> GetCampaign()
        {
            APIResponse<IEnumerable<GetCampaignAndStageResponse>> response = new();
            var campaign = await dbContext.Campaigns.Where(p => p.Status == true).ToListAsync();
            List<GetCampaignAndStageResponse> listCamn = new List<GetCampaignAndStageResponse>();
            foreach (var item in campaign)
            {
                var map = _mapper.Map<GetCampaignAndStageResponse>(item);
                var element = await dbContext.Stages.Where(p => p.CampaignId == item.CampaignId).ToListAsync();
                List<GetStageResponse> listStage = element.Select(
               x =>
               {
                   return new GetStageResponse()
                   {
                       StageId = x.StageId,
                       Content = x.Content,
                       Title = x.Title,
                       Description = x.Description,
                       StartTime = x.StartTime,
                       EndTime = x.EndTime,
                       CampaignId = x.CampaignId,
                       FormId = x.FormId,
                   };
               }
               ).ToList();
                map.Stage = listStage;

                listCamn.Add(map);
            }
            response.Data = listCamn;
            if (response.Data == null)
            {
                response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách Campaign thành công", StatusCodes.Status200OK);
            return response;
        }
        public async Task<APIResponse<GetCampaignAndStageResponse>> GetCampaignById(Guid id)
        {
            APIResponse<GetCampaignAndStageResponse> response = new();
            var getById = await dbContext.Campaigns.Where(p => p.CampaignId == id && p.Status == true)
                .SingleOrDefaultAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var map = _mapper.Map<GetCampaignAndStageResponse>(getById);

            var campaign = await dbContext.Stages.Where(p => p.CampaignId == getById.CampaignId).ToListAsync();
            List<GetStageResponse> listStage = new List<GetStageResponse>();
            foreach (var item in campaign)
            {
                GetStageResponse stage = new GetStageResponse();
                stage.CampaignId = item.CampaignId;
                stage.Title = item.Title;
                stage.Description = item.Description;
                stage.Content = item.Content;
                stage.StartTime = item.StartTime;
                stage.EndTime = item.EndTime;
                stage.FormId = item.FormId;
                listStage.Add(stage);
            }
            map.Stage = listStage;
            response.Data = map;
            if (response.Data == null)
            {
                response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy chi tiết Campaign thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignByUserId(string uid)
        {
            APIResponse<IEnumerable<GetCampaignResponse>> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == uid && p.Status == true)
               .SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("User không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var getById = await dbContext.Campaigns.Where(p => p.UserId == uid && p.Status == true)
                .ToListAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetCampaignResponse> result = getById.Select(
               x =>
               {
                   return new GetCampaignResponse()
                   {
                       CategoryId = x.CategoryId,
                       Title = x.Title,
                       StartTime = x.StartTime,
                       EndTime = x.EndTime,
                       Visibility = x.Visibility,
                       ImgUrl = x.ImgUrl,
                       Status = x.Status,
                       UserId = x.UserId,
                       CampaignId = x.CampaignId,
                   };
               }
               ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
            //var map = _mapper.Map<GetCampaignAndStageResponse>(getById);

            //var campaign = await dbContext.Stages.Where(p => p.CampaignId == getById.CampaignId).ToListAsync();
            //List<GetStageResponse> listStage = new List<GetStageResponse>();
            //foreach (var item in campaign)
            //{
            //    GetStageResponse stage = new GetStageResponse();
            //    stage.CampaignId = item.CampaignId;
            //    stage.Title = item.Title;
            //    stage.Description = item.Description;
            //    stage.Content = item.Content;
            //    stage.StartTime = item.StartTime;
            //    stage.EndTime = item.EndTime;
            //    stage.FormId = item.FormId;
            //    listStage.Add(stage);
            //}
            //map.Stage = listStage;
            //response.Data = map;
            //if (response.Data == null)
            //{
            //    response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
            //    return response;
            //}
            //response.ToSuccessResponse(response.Data, "Lấy chi tiết Campaign thành công", StatusCodes.Status200OK);
            //return response;
        }

        public async Task<APIResponse<GetCampaignResponse>> UpdateCampaign(Guid id, UpdateCampaignRequest request)
        {
            APIResponse<GetCampaignResponse> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == id);
            if (cam == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var category = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (cam == null)
            {
                response.ToFailedResponse("Category không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.StartTime = request.StartTime;
            cam.EndTime = request.EndTime;
            //cam.Status = request.Status;
            cam.Visibility = request.Visibility;
            cam.Title = request.Title;
            cam.CategoryId = request.CategoryId;
            cam.ImgUrl = request.ImgUrl;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<GetCampaignResponse>> UpdateVisibilityCampaign(Guid id, string request, string us)
        {
            APIResponse<GetCampaignResponse> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == id);
            if (cam == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var cam1 = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == id && c.UserId.Equals(us));
            if (cam1 == null)
            {
                response.ToFailedResponse("User này không phải người tạo ra Campaign này", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Equals("public") && !request.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.Visibility = request;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
