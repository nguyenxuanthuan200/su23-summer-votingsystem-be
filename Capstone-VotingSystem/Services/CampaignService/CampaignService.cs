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
            var cam = await dbContext.Campaigns.Where(p => p.Status == true && p.CampaignId == id).SingleOrDefaultAsync();
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            if (cam.IsApprove == true)
            {
                response.ToFailedResponse("Chiến dịch đã được xác nhận", StatusCodes.Status400BadRequest);
                return response;
            }
            //check campaign du dieu kien chua
            var checkCandidate = await dbContext.Candidates.Where(p => p.Status == true && p.CampaignId == id).ToListAsync();
            if (checkCandidate.Count <= 1)
            {
                response.ToFailedResponse("Số lượng ứng viên của chiến dịch không thể ít hơn 2", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkStage = await dbContext.Stages.Where(p => p.Status == true && p.CampaignId == id).ToListAsync();
            if (checkStage.Count <= 0)
            {
                response.ToFailedResponse("Số giai đoạn của chiến dịch không thể ít hơn 1", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkGroupCandidate = await dbContext.Groups.Where(p => p.CampaignId == id && p.IsVoter == false).ToListAsync();
            if (checkGroupCandidate.Count <= 0)
            {
                response.ToFailedResponse("Nhóm của ứng cử viên không thể trống", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkGroupVoter = await dbContext.Groups.Where(p => p.CampaignId == id && p.IsVoter == true && p.IsStudentMajor == false).ToListAsync();
            if (checkGroupVoter.Count <= 0)
            {
                response.ToFailedResponse("Nhóm của người bình chọn không thể trống", StatusCodes.Status400BadRequest);
                return response;
            }
            //gui thong bao cho toan bo nguoi dung trong he thong khi co chien dich moi
            if (cam.Visibility == "public")
            {
                TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
                var listUser = dbContext.Users.Where(p => p.Status == true).AsQueryable();
                listUser = listUser.Where(p => p.Permission == 0 || p.Permission == 3 || p.Permission == 5 || p.Permission == 6);
                listUser = listUser.Where(p => p.UserId != cam.UserId);
                foreach (var user in listUser)
                {
                    Guid idNoti = Guid.NewGuid();
                    Notification noti = new Notification()
                    {
                        NotificationId = idNoti,
                        Title = "Thông báo chiến dịch mới",
                        Message = "Có một chiến dịch mới - " + cam.Title + " vừa được tạo, mời bạn tham gia bình chọn. Chiến dịch bắt đầu vào ngày:" + cam.StartTime.ToString("dd/MM/yyyy") + " hãy nhớ tham gia nhé!!",
                        CreateDate = currentDateTimeVn,
                        IsRead = false,
                        Status = true,
                        Username = user.UserId,
                        CampaignId = cam.CampaignId,
                    };
                    await dbContext.Notifications.AddAsync(noti);
                    // await dbContext.SaveChangesAsync();

                }
            }
            cam.IsApprove = true;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Xác nhận chiến dịch thành công", StatusCodes.Status200OK);
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
                response.ToFailedResponse("Thời gian bắt đầu phải sau thời gian hiện tại và thời gian kết thúc phải sau thời gian bắt đầu.", StatusCodes.Status400BadRequest);
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
                cam.Process = "Chưa bắt đầu";
                cam.IsApprove = false;
                cam.Status = true;
                cam.UserId = request.UserId;
                cam.CategoryId = request.CategoryId;
                cam.VisibilityCandidate = request.VisibilityCandidate;
                cam.Representative = false;
                cam.PublishTheResult = true;

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
            var checkrole = await dbContext.Roles.Where(p => p.RoleId == checkus.RoleId).SingleOrDefaultAsync();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == CampaignId);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (cam.UserId != request.UserName && checkrole.Name != "admin")
            {
                response.ToFailedResponse("Bạn không đủ quyền để xóa chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkrole.Name == "admin")
            {
                TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
                Guid idNoti = Guid.NewGuid();
                Notification noti = new Notification()
                {
                    NotificationId = idNoti,
                    Title = "Thông báo chiến dịch",
                    Message = "Chiến dịch - " + cam.Title + "của bạn vừa bị xóa bởi admin vì vi phạm điều lệ, vui lòng liên hệ để biết thêm thông tin chi tiết.",
                    CreateDate = currentDateTimeVn,
                    IsRead = false,
                    Status = true,
                    Username = cam.UserId,
                    CampaignId = cam.CampaignId,
                };
                await dbContext.Notifications.AddAsync(noti);
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
            if (campaign.Count() <= 0)
            {
                response.ToFailedResponse("Không có chiến dịch nào", StatusCodes.Status400BadRequest);
                return response;
            }
            List<GetCampaignAndStageResponse> listCamn = new List<GetCampaignAndStageResponse>();
            foreach (var item in campaign)
            {
                var map = _mapper.Map<GetCampaignAndStageResponse>(item);

                var totalCandidate = await dbContext.Candidates.Where(p => p.CampaignId == item.CampaignId && p.Status == true).ToListAsync();
                map.TotalCandidate = totalCandidate.Count();
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

            response.ToSuccessResponse(response.Data, "Lấy danh sách chiến dịch thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignResponse>>> GetCampaignNeedApprove()
        {
            APIResponse<IEnumerable<GetCampaignResponse>> response = new();

            var getById = await dbContext.Campaigns.Where(p => p.IsApprove == false && p.Status == true)
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
                       IsApprove = x.IsApprove,
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
            List<GetCampaignResponse> listCamn = new List<GetCampaignResponse>();
            foreach (var item in getById)
            {
                var map = _mapper.Map<GetCampaignResponse>(item);

                var totalCandidate = await dbContext.Candidates.Where(p => p.CampaignId == item.CampaignId && p.Status == true).ToListAsync();
                map.TotalCandidate = totalCandidate.Count();
                listCamn.Add(map);
            }

            response.Data = listCamn;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
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
            if (cam.Process != "Chưa bắt đầu" && cam.IsApprove == true)
            {
                response.ToFailedResponse("Bạn chỉ có thể chỉnh sửa khi chiến dịch chưa bắt đầu và khi chưa xác nhận điều khoản", StatusCodes.Status400BadRequest);
                return response;
            }
            var category = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == request.CategoryId);
            if (category == null)
            {
                response.ToFailedResponse("Thể loại của chiến dịch không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (!request.Visibility.Equals("public") && !request.Visibility.Equals("private"))
            {
                response.ToFailedResponse("Trạng thái không đúng định dạng!! (công khai hoặc không công khai)", StatusCodes.Status400BadRequest);
                return response;
            }
            var uploadResult = new ImageUploadResult();
            if (request.ImageFile != null && request.ImageFile.Length > 0)
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
                response.ToFailedResponse("Thời gian bắt đầu phải sau thời gian hiện tại và thời gian kết thúc phải sau thời gian bắt đầu.", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.StartTime = request.StartTime;
            cam.EndTime = request.EndTime;
            //cam.Status = request.Status;
            cam.Visibility = request.Visibility;
            cam.Title = request.Title;
            cam.CategoryId = request.CategoryId;
            cam.VisibilityCandidate = request.VisibilityCandidate;
            cam.ImgUrl = uploadResult.SecureUrl?.AbsoluteUri ?? cam.ImgUrl;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            response.ToSuccessResponse("Cập nhật chiến dịch thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<ImageCampaignResponse>> AddImageCampaignAsync(IFormFile file, string folderName, Guid? campaignId)
        {
            APIResponse<ImageCampaignResponse> response = new();
            var checkCampaign = await dbContext.Campaigns.Where(p => p.CampaignId.Equals(campaignId)).SingleOrDefaultAsync();
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại", StatusCodes.Status404NotFound);
                return response;
            }
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Crop("fill").Gravity("face"),
                    Folder = folderName,

                };

                uploadResult = await _cloudinary.UploadAsync(uploadParams);

            }
            checkCampaign.ImgUrl = uploadResult.SecureUrl.AbsoluteUri;

            ImageCampaignResponse imageRes = new ImageCampaignResponse();
            {
                imageRes.CampaignId = checkCampaign.CampaignId;
                imageRes.AvatarURL = uploadResult.SecureUrl.AbsoluteUri;
                imageRes.PublicId = uploadResult.PublicId;
            }
            dbContext.Campaigns.Update(checkCampaign);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse(imageRes, "Cập nhật thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> UpdateProcess()
        {
            APIResponse<string> response = new();
            var listCampaign = await dbContext.Campaigns.Where(p => p.Status == true && p.IsApprove == true).ToListAsync();
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            foreach (var campaign in listCampaign)
            {
                var listStage = await dbContext.Stages.Where(p => p.CampaignId == campaign.CampaignId && p.Status == true).ToListAsync();
                if (listStage.Count > 0)
                {
                    foreach (var stage in listStage)
                    {
                        if (stage.Process == "Chưa bắt đầu")
                        {


                            if (DateTime.Compare(stage.StartTime, currentDateTimeVn) <= 0)
                            {
                                stage.Process = "Đang diễn ra";
                                dbContext.Stages.Update(stage);
                            }

                        }
                        else if (stage.Process == "Đang diễn ra")
                        {


                            if (DateTime.Compare(stage.EndTime, currentDateTimeVn) <= 0)
                            {
                                stage.Process = "Đã kết thúc";
                                dbContext.Stages.Update(stage);
                            }

                        }
                    }
                    if (campaign.Process == "Chưa bắt đầu")
                    {
                        if (DateTime.Compare(campaign.StartTime, currentDateTimeVn) <= 0)
                        {
                            campaign.Process = "Đang diễn ra";
                            dbContext.Campaigns.Update(campaign);
                        }
                    }
                    else if (campaign.Process == "Đang diễn ra")
                    {
                        if (DateTime.Compare(campaign.EndTime, currentDateTimeVn) <= 0)
                        {
                            campaign.Process = "Đã kết thúc";
                            dbContext.Campaigns.Update(campaign);
                        }
                    }
                }

            }
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Cập nhật trạng thái thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetCampaignAndStageResponse>> GetCampaignRepresentative()
        {
            APIResponse<GetCampaignAndStageResponse> response = new();
            var getById = await dbContext.Campaigns.Where(p => p.Status == true && p.Representative == true)
                .SingleOrDefaultAsync();
            if (getById == null)
            {
                response.ToFailedResponse("Không có chiến dịch tiêu biểu nào", StatusCodes.Status400BadRequest);
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
            response.ToSuccessResponse(response.Data, "Lấy chiến dịch tiêu biểu thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> UpdateCampaignRepresentative(Guid id)
        {
            APIResponse<string> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == id);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (cam.Representative == true)
            {
                response.ToFailedResponse("Chiến dịch đã là tiêu biểu", StatusCodes.Status400BadRequest);
                return response;
            }
            if (cam.IsApprove == false)
            {
                response.ToFailedResponse("Chiến dịch chưa xác nhận chính sách", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkCamReUpdate = await dbContext.Campaigns.Where(p => p.Status == false && p.Representative == true).ToListAsync();
            if (checkCamReUpdate.Count > 0)
            {
                foreach (var item in checkCamReUpdate)
                {
                    item.Representative = false;
                    dbContext.Campaigns.Update(item);
                    await dbContext.SaveChangesAsync();
                }
            }
            var checkCamRe = await dbContext.Campaigns.Where(p => p.Status == true && p.Representative == true).SingleOrDefaultAsync();
            if (checkCamRe != null)
            {
                checkCamRe.Representative = false;
                cam.Representative = true;
                dbContext.Campaigns.Update(checkCamRe);
                dbContext.Campaigns.Update(cam);
                await dbContext.SaveChangesAsync();
                response.ToSuccessResponse("Thay đổi chiến dịch tiêu biểu thành công", StatusCodes.Status200OK);
                return response;
            }
            else
            {
                cam.Representative = true;
                dbContext.Campaigns.Update(cam);
                await dbContext.SaveChangesAsync();
                response.ToSuccessResponse("Thay đổi chiến dịch tiêu biểu thành công", StatusCodes.Status200OK);
                return response;
            }
        }

        public async Task<APIResponse<IEnumerable<GetCampaignForAdminResponse>>> GetCampaignForAdmin()
        {
            APIResponse<IEnumerable<GetCampaignForAdminResponse>> response = new();

            var getAll = await dbContext.Campaigns.ToListAsync();
            if (getAll == null || getAll.Count == 0)
            {
                response.ToFailedResponse("Không có chiến dịch nào tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            List<GetCampaignForAdminResponse> listCamn = new List<GetCampaignForAdminResponse>();
            foreach (var item in getAll)
            {
                var map = _mapper.Map<GetCampaignForAdminResponse>(item);

                //var totalCandidate = await dbContext.Candidates.Where(p => p.CampaignId == item.CampaignId && p.Status == true).ToListAsync();
                //map.TotalCandidate = totalCandidate.Count();
                listCamn.Add(map);
            }

            response.Data = listCamn;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> UnDeleteCampaign(Guid CampaignId)
        {
            APIResponse<string> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == CampaignId);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch vẫn còn hoạt động", StatusCodes.Status400BadRequest);
                return response;
            }
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            Guid idNoti = Guid.NewGuid();
            Notification noti = new Notification()
            {
                NotificationId = idNoti,
                Title = "Thông báo chiến dịch",
                Message = "Chiến dịch - " + cam.Title + "của bạn vừa được gỡ lệnh cấm.",
                CreateDate = currentDateTimeVn,
                IsRead = false,
                Status = true,
                Username = cam.UserId,
                CampaignId = cam.CampaignId,
            };
            cam.Status = true;
            dbContext.Campaigns.Update(cam);
            await dbContext.Notifications.AddAsync(noti);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Bỏ gỡ chiến dịch thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> UpdatePublicResult(Guid campaignId)
        {
            APIResponse<string> response = new();
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == campaignId);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (cam.PublishTheResult == true)
                cam.PublishTheResult = false;
            else
                cam.PublishTheResult = true;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Thay đổi thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
