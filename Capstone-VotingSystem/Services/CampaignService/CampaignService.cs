using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.StageResponse;
using Capstone_VotingSystem.Helpers;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Capstone_VotingSystem.Services.CampaignService
{
    public class CampaignService : ICampaignService
    {
        private readonly Cloudinary _cloudinary;
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public CampaignService(VotingSystemContext dbContext, IMapper mapper, IOptions<CloudinarySettings> config)
        {
            var acc = new CloudinaryDotNet.Account(
            config.Value.CloundName,
            config.Value.ApiKey,
            config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<GetCampaignResponse>> ApproveCampaign(Guid id)
        {
            APIResponse<GetCampaignResponse> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true &&p.IsApprove ==false&&p.CampaignId==id).SingleOrDefaultAsync();
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch đã bị xóa hoặc đã được duyệt", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.IsApprove = true;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Duyệt chiến dịch thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<GetCampaignResponse>> CreateCampaign(CreateCampaignRequest request)
        {
            APIResponse<GetCampaignResponse> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(c => c.UserId == request.UserId);
            if (checkUser == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkCategory = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (checkCategory == null)
            {
                response.ToFailedResponse("Thể loại không tồn tại", StatusCodes.Status400BadRequest);
                return
                response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }
            #region
            //string StartDateString = ""+request.StartTime;
            //string EndDateString = ""+request.EndTime;
            //DateTime dateToCheck;

            //if (DateTime.TryParseExact(StartDateString, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateToCheck) && dateToCheck.CompareTo(DateTime.UtcNow) > 0)
            //{
            //    DateTime startDate;
            //    DateTime endDate;

            //    if (DateTime.TryParseExact(StartDateString, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate) &&
            //        DateTime.TryParseExact(EndDateString, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate) &&
            //        endDate.CompareTo(startDate) > 0)
            //    {

            //    }
            //    else
            //    {
            //        response.ToFailedResponse("Thời gian kết thúc chiến dịch phải lớn hơn thời gian bắt đầu chiến dịch", StatusCodes.Status400BadRequest);
            //        return response;
            //    }
            //}
            //else
            //{
            //    // dateToCheck không hợp lệ hoặc nhỏ hơn hoặc bằng ngày giờ hiện tại (theo múi giờ UTC)
            //    response.ToFailedResponse("Thời gian bắt đầu chiến dịch phải lớn hơn thời gian hiện tại ", StatusCodes.Status400BadRequest);
            //    return response;
            //}
            #endregion
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);

            // Kiểm tra giá trị của hai biến DateTime
            if (DateTime.Compare(request.StartTime, currentDateTimeVn) > 0 && DateTime.Compare(request.EndTime, request.StartTime) > 0)
            {
                //Console.WriteLine("Start time is after current time, and end time is after start time.");
            }
            else
            {
                response.ToFailedResponse("Thời gian bắt đầu không sau thời gian hiện tại hoặc thời gian kết thúc không sau thời gian bắt đầu.", StatusCodes.Status400BadRequest);
                return response;
            }
            var uploadResult = new ImageUploadResult();
            if (request.ImageFile.Length > 0)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(request.ImageFile.FileName, request.ImageFile.OpenReadStream()),
                    Transformation = new Transformation().Crop("fill").Gravity("face"),
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            var id = Guid.NewGuid();
            Campaign cam = new Campaign();
            {
                cam.CampaignId = id;
                cam.Title = request.Title;
                cam.StartTime = request.StartTime;
                cam.EndTime = request.EndTime;
                cam.Visibility = request.Visibility;
                cam.ImgUrl = uploadResult.SecureUrl.AbsoluteUri;
                cam.Process = "chưa bắt đầu";
                cam.IsApprove = false;
                cam.Status = true;
                cam.UserId = request.UserId;
                cam.CategoryId = request.CategoryId;
                
            };
            await dbContext.Campaigns.AddAsync(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Tạo chiến dịch thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteCampaign(Guid CampaignId, DeleteCampaignRequest request)
        {
            APIResponse<string> response = new();
           
            var checkus = await dbContext.Accounts.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.UserName == request.UserName);
            if (checkus == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var cam = await dbContext.Campaigns.Where(p => p.Status == true&& p.UserId==request.UserName).SingleOrDefaultAsync(c => c.CampaignId == CampaignId);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.Status = false;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            //var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Xóa chiến dịch thành công", StatusCodes.Status200OK);
            //response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignAndStageResponse>>> GetCampaign()
        {
            APIResponse<IEnumerable<GetCampaignAndStageResponse>> response = new();
            var campaign = await dbContext.Campaigns.Where(p => p.Status == true && p.Visibility == "public" && p.IsApprove == true).ToListAsync();

            List<GetCampaignAndStageResponse> listCamn = new List<GetCampaignAndStageResponse>();
            foreach (var item in campaign)
            {
                var map = _mapper.Map<GetCampaignAndStageResponse>(item);
                var stage = await dbContext.Stages.Where(p => p.CampaignId == item.CampaignId).ToListAsync();

                //if (element.Exists(image => string.IsNullOrEmpty(item.Title)))
                //{
                //    response.ToFailedResponse("Không có Campaign nào", StatusCodes.Status400BadRequest);
                //    return response;
                //}
                if (stage.Count != 0)
                {
                    List<GetStageResponse> listStage = stage.Select(
                   x =>
                   {
                       return new GetStageResponse()
                       {
                           StageId = x.StageId,
                           Content = x.Content,
                           Title = x.Title,
                           Description = x.Description,
                           StartTime = x.StartTime,
                           Process = x.Process,
                           LimitVote = x.LimitVote,
                           IsUseForm = x.IsUseForm,
                           EndTime = x.EndTime,
                           CampaignId = x.CampaignId,
                           FormId = x.FormId,
                       };
                   }
                   ).ToList();
                    map.Stage = listStage;

                    listCamn.Add(map);
                }
            }
            response.Data = listCamn;
            if (response.Data == null)
            {
                response.ToFailedResponse("Không có chiến dịch nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách chiến dịch thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignNeedApprove()
        {
            APIResponse<IEnumerable<GetCampaignResponse>> response = new();

            var getById = await dbContext.Campaigns.Where(p => p.IsApprove ==false && p.Status == true)
                .ToListAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Không có chiến dịch nào cần duyệt", StatusCodes.Status400BadRequest);
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
                       Process = x.Process,
                       IsApporve = x.IsApprove,
                       Status = x.Status,
                       UserId = x.UserId,
                       CampaignId = x.CampaignId,
                   };
               }
               ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetCampaignAndStageResponse>> GetCampaignById(Guid id)
        {
            APIResponse<GetCampaignAndStageResponse> response = new();
            var getById = await dbContext.Campaigns.Where(p => p.CampaignId == id && p.Status == true)
                .SingleOrDefaultAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var map = _mapper.Map<GetCampaignAndStageResponse>(getById);

            var liststage = await dbContext.Stages.Where(p => p.CampaignId == getById.CampaignId).ToListAsync();

            List<GetStageResponse> listStagee = new List<GetStageResponse>();
            foreach (var item in liststage)
            {

                GetStageResponse stage = new GetStageResponse();
                stage.StageId = item.StageId;
                stage.Title = item.Title;
                stage.Description = item.Description;
                stage.Content = item.Content;
                stage.StartTime = item.StartTime;
                stage.EndTime = item.EndTime;
                stage.Process = item.Process;
                stage.LimitVote = item.LimitVote;
                stage.IsUseForm = item.IsUseForm;
                stage.CampaignId = item.CampaignId;
                stage.FormId = item.FormId;
                listStagee.Add(stage);
            }
            map.Stage = listStagee;
            response.Data = map;
            if (response.Data == null)
            {
                response.ToFailedResponse("Không có chiến dịch nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy chi tiết chiến dịch thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignByUserId(string uid)
        {
            APIResponse<IEnumerable<GetCampaignResponse>> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == uid && p.Status == true)
               .SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Người dùng không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var getById = await dbContext.Campaigns.Where(p => p.UserId == uid && p.Status == true)
                .ToListAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
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
                       Process = x.Process,
                       IsApporve = x.IsApprove,
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
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var category = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (cam == null)
            {
                response.ToFailedResponse("Thể loại không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Visibility không đúng định dạng!! (public or private)", StatusCodes.Status400BadRequest);
                return response;
            }
            var uploadResult = new ImageUploadResult();
            if (request.ImageFile.Length > 0)
            {
                var uploadParams = new ImageUploadParams()
                {
                    //PublicId = "existing_public_id",
                    //Overwrite = true,
                    File = new FileDescription(request.ImageFile.FileName, request.ImageFile.OpenReadStream()),
                    Transformation = new Transformation().Crop("fill").Gravity("face"),
                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);

            // Kiểm tra giá trị của hai biến DateTime
            if (DateTime.Compare(request.StartTime, currentDateTimeVn) > 0 && DateTime.Compare(request.EndTime, request.StartTime) > 0)
            {
                //Console.WriteLine("Start time is after current time, and end time is after start time.");
            }
            else
            {
                response.ToFailedResponse("Thời gian bắt đầu không sau thời gian hiện tại hoặc thời gian kết thúc không sau thời gian bắt đầu.", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.StartTime = request.StartTime;
            cam.EndTime = request.EndTime;
            //cam.Status = request.Status;
            cam.Visibility = request.Visibility;
            cam.Title = request.Title;
            cam.CategoryId = request.CategoryId;
            cam.ImgUrl = uploadResult.SecureUrl.AbsoluteUri;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Cập nhật chiến dịch thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

    }
}
