using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.StageRequest;
using Capstone_VotingSystem.Models.ResponseModels.StageResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.StageService
{
    public class StageService : IStageService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public StageService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<CreateStageResponse>> CreateCampaignStage(CreateStageRequest request)
        {
            APIResponse<CreateStageResponse> response = new();
            var campaign = await dbContext.Campaigns.Where(
             p => p.CampaignId == request.CampaignId).SingleOrDefaultAsync();

            if (campaign == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.FormId !=null && request.FormId != Guid.Empty)
            {
                var checkForm = await dbContext.Forms.Where(p => p.FormId == request.FormId).SingleOrDefaultAsync();
                if (checkForm == null)
                {
                    response.ToFailedResponse("Biểu mẫu không tồn tại", StatusCodes.Status400BadRequest);
                    return response;
                }
            }

            DateTime startTime = (DateTime)campaign.StartTime;
            DateTime endTime = (DateTime)campaign.EndTime;
            DateTime newStartTime = (DateTime)request.StartTime;
            DateTime newEndTime = (DateTime)request.EndTime;
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            if (DateTime.Compare(newEndTime, newStartTime) <= 0)
            {
                response.ToFailedResponse("Thời gian bắt đầu không sau thời gian hiện tại hoặc thời gian kết thúc không sau thời gian bắt đầu.", StatusCodes.Status400BadRequest);
                return response;
            }
            // Kiểm tra giá trị của hai biến DateTime mới
            if (DateTime.Compare(newStartTime, startTime) >= 0 && DateTime.Compare(newEndTime, endTime) <= 0)
            {
                //Console.WriteLine("New start time and new end time are within the range of start time and end time.");
            }
            else
            {
                response.ToFailedResponse("Thời gian bắt đầu của giai đoạn và thời gian kết thúc của giai đoạn không nằm trong phạm vi của thời gian bắt đầu và thời gian kết thúc của chiến dịch.", StatusCodes.Status400BadRequest);
                return response;
            }

            var id = Guid.NewGuid();
            Stage stage = new Stage();
            {
                stage.StageId = id;
                stage.CampaignId = request.CampaignId;
                stage.Description = request.Description;
                stage.Title = request.Title;
                stage.Content = request.Content;
                if (request.FormId != null)
                {
                    stage.FormId = request.FormId;
                    stage.IsUseForm = true;
                }
                else
                {
                    stage.FormId = null;
                    stage.IsUseForm = false;
                }
                stage.StartTime = request.StartTime;
                stage.EndTime = request.EndTime;
                stage.Status = true;
                stage.Process = "Chưa bắt đầu";
                stage.LimitVote = request.LimitVote;
            };
            await dbContext.Stages.AddAsync(stage);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateStageResponse>(stage);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> DeleteStage(Guid id)
        {
            APIResponse<string> response = new();
            var stage = await dbContext.Stages.Where(p => p.StageId == id && p.Status == true).SingleOrDefaultAsync();
            if (stage == null)
            {
                response.ToFailedResponse("Không đúng giai đoạn hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkCampaignApprove = await dbContext.Campaigns.Where(p => p.CampaignId == stage.CampaignId && p.Status == true && p.IsApprove == false).SingleOrDefaultAsync();
            if (checkCampaignApprove == null)
            {
                response.ToFailedResponse("Bạn chỉ có thể thực hiện chức năng này khi chiến dịch chưa được duyệt", StatusCodes.Status400BadRequest);
                return response;
            }
            stage.Status = false;
            dbContext.Stages.Update(stage);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa giai đoạn thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetStageResponse>>> GetCampaignStageByCampaign(Guid campaignId)
        {
            APIResponse<IEnumerable<GetStageResponse>> response = new();
            var campaign = await dbContext.Campaigns.Where(p => p.CampaignId == campaignId).ToListAsync();
            if (campaign == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }

            var campaignstage = await dbContext.Stages.Where(p => p.CampaignId == campaignId && p.Status == true).ToListAsync();
            if (campaignstage == null)
            {
                response.ToFailedResponse("Không có Stage nào trong Campaign này", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetStageResponse> result = campaignstage.Select(
                x =>
                {
                    return new GetStageResponse()
                    {
                        StageId = x.StageId,
                        CampaignId = x.CampaignId,
                        Description = x.Description,
                        Title = x.Title,
                        StartTime = x.StartTime,
                        EndTime = x.StartTime,
                        LimitVote = x.LimitVote,
                        Process = x.Process,
                        Content = x.Content,
                        FormId = x.FormId,

                    };
                }
                ).ToList();
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<GetStageResponse>> UpdateCampaignStage(Guid id, UpdateStageRequest request)
        {
            APIResponse<GetStageResponse> response = new();
            var upStage = await dbContext.Stages.SingleOrDefaultAsync(c => c.StageId == id);
            if (upStage == null)
            {
                response.ToFailedResponse("Stage không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcampaign = await dbContext.Campaigns.SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId);
            if (checkcampaign == null)
            {
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.FormId != null)
            {
                var checkform = await dbContext.Forms.SingleOrDefaultAsync(c => c.FormId == request.FormId);
                if (checkform == null)
                {
                    response.ToFailedResponse("Form không tồn tại", StatusCodes.Status400BadRequest);
                    return response;
                }
            }
            if (!request.Process.Equals("Chưa diễn ra") && !request.Process.Equals("Đang diễn ra") && !request.Process.Equals("Đã kết thúc"))
            {
                response.ToFailedResponse("Process không đúng định dạng!! (Chưa diễn ra hoặc Đang diễn ra hoặc Đã kết thúc )", StatusCodes.Status400BadRequest);
                return response;
            }

            upStage.Description = request.Description;
            upStage.Title = request.Title;
            upStage.StartTime = request.StartTime;
            upStage.EndTime = request.EndTime;
            upStage.Content = request.Content;
            upStage.LimitVote = request.LimitVote;
            upStage.Process = request.Process;
            upStage.FormId = request.FormId;
            dbContext.Stages.Update(upStage);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetStageResponse>(upStage);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;

        }

    }
}
