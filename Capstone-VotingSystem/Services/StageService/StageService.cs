﻿using AutoMapper;
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
                response.ToFailedResponse("Campaign không tồn tại", StatusCodes.Status400BadRequest);
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
                stage.FormId = request.FormId;
                stage.StartTime = request.StartTime;
                stage.EndTime = request.EndTime;
            };
            await dbContext.Stages.AddAsync(stage);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateStageResponse>(stage);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
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

            var campaignstage = await dbContext.Stages.Where(p => p.CampaignId == campaignId).ToListAsync();
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
            upStage.Description = request.Description;
            upStage.Title = request.Title;
            upStage.StartTime = request.StartTime;
            upStage.EndTime = request.EndTime;
            upStage.Content = request.Content;
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
