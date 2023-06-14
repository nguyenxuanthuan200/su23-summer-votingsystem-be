using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.CampaignStageService
{
    public class CampaignStageService : ICampaignStageService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public CampaignStageService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<CreateCampaginStageResponse>> CreateCampaignStage(CreateCampaignStageRequest request)
        {
            //APIResponse<CreateCampaginStageResponse> response = new();
            //var campaign = await dbContext.Campaigns.Where(
            // p => p.CampaignId == request.CampaignId).SingleOrDefaultAsync();

            //if (campaign == null)
            //{
            //    response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
            //    return response;
            //}
            //var id = Guid.NewGuid();
            //CampaignStage campaignStage = new CampaignStage();
            //{
            //    campaignStage.CampaignStageId = id;
            //    campaignStage.CampaignId = request.CampaignId;
            //    campaignStage.Description = request.Description;
            //    campaignStage.Title = request.Title;
            //    campaignStage.Status = request.Status;
            //    campaignStage.Text = request.Text;
            //    campaignStage.StartTime = request.StartTime;
            //    campaignStage.EndTime = request.EndTime;
            //};
            //await dbContext.CampaignStages.AddAsync(campaignStage);
            //await dbContext.SaveChangesAsync();
            //var map = _mapper.Map<CreateCampaginStageResponse>(campaignStage);
            //response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            //response.Data = map;
            //return response;
            return null;
        }

        public async Task<APIResponse<IEnumerable<GetCampaignStageByCampaignResponse>>> GetCampaignStageByCampaign(Guid campaignId)
        {
            APIResponse<IEnumerable<GetCampaignStageByCampaignResponse>> response = new();
            var campaignstage = await dbContext.Stages.Where(p => p.CampaignId == campaignId).ToListAsync();
            if (campaignstage == null)
            {
                response.ToFailedResponse("Không có CampaignStage nào", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetCampaignStageByCampaignResponse> result = campaignstage.Select(
                x =>
                {
                    return new GetCampaignStageByCampaignResponse()
                    {
                        CampaignStageId = x.StageId,
                        CampaignId = x.CampaignId,
                        Description = x.Description,
                        Title = x.Title,
                        StartTime = x.StartTime,
                        EndTime = x.StartTime,
                       // Text = x.Text,
                        //Status = x.Status,

                    };
                }
                ).ToList();
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<GetCampaignStageByCampaignResponse>> UpdateCampaignStage(Guid id, UpdateCampaignStageRequest request)
        {
            APIResponse<GetCampaignStageByCampaignResponse> response = new();
            var upCam = await dbContext.Stages.SingleOrDefaultAsync(c => c.StageId == id);
            if (upCam == null)
            {
                response.ToFailedResponse("CampaignStage không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcampaign = await dbContext.Campaigns.SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId);
            if (checkcampaign == null)
            {
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            upCam.Description = upCam.Description;
            upCam.Title = upCam.Title;
            upCam.StartTime = upCam.StartTime;
            upCam.EndTime = upCam.EndTime;
            //upCam.Text = upCam.Text;
            //upCam.Status = upCam.Status;
            dbContext.Stages.Update(upCam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignStageByCampaignResponse>(upCam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;

        }

    }
}
