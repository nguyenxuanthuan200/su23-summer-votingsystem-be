﻿using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.RatioRequest;
using Capstone_VotingSystem.Models.ResponseModels.RatioResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.RatioService
{
    public class RatioService : IRatioService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public RatioService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<RatioResponse>> CreateCampaignRatio(CreateRatioRequest request)
        {
            APIResponse<RatioResponse> response = new();
            var checkCampaign = await dbContext.Campaigns.SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId && c.Status == true);
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkGroup = await dbContext.Groups.SingleOrDefaultAsync(c => c.GroupId == request.GroupVoterId && c.IsVoter == true);
            if (checkGroup == null)
            {
                response.ToFailedResponse("Nhóm người bình chọn không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkGroup1 = await dbContext.Groups.SingleOrDefaultAsync(c => c.GroupId == request.GroupCandidateId && c.IsVoter == false);
            if (checkGroup1 == null)
            {
                response.ToFailedResponse("Nhóm ứng cử viên không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkRatio = await dbContext.Ratios.SingleOrDefaultAsync(c => c.GroupVoterId == request.GroupVoterId && c.CampaignId == request.CampaignId && c.GroupCandidateId == request.GroupCandidateId);
            if (checkRatio != null)
            {
                checkRatio.Proportion=request.Proportion;
                dbContext.Ratios.Update(checkRatio);
                await dbContext.SaveChangesAsync();
               // var map = _mapper.Map<RatioResponse>(checkRatio);
                response.ToSuccessResponse("Tạo trọng số thành công", StatusCodes.Status200OK);
              //  response.Data = map;
                return response;
            }
            var id = Guid.NewGuid();
            Ratio ratio = new Ratio();
            {
                ratio.RatioGroupId = id;
                ratio.Proportion = request.Proportion;
                ratio.GroupVoterId = request.GroupVoterId;
                ratio.CampaignId = request.CampaignId;
                ratio.GroupCandidateId = request.GroupCandidateId;
            };
            await dbContext.Ratios.AddAsync(ratio);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<RatioResponse>(ratio);
            response.ToSuccessResponse("Tạo trọng số thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<RatioResponse>>> GetRatioByCampaign(Guid campaignId)
        {
            APIResponse<IEnumerable<RatioResponse>> response = new();
            var check = await dbContext.Campaigns.Where(p => p.CampaignId == campaignId && p.Status == true)
               .SingleOrDefaultAsync();
            if (check == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var getById = await dbContext.Ratios.Where(p => p.CampaignId == campaignId).ToListAsync();
            if (getById == null || getById.Count == 0)
            {
                response.ToFailedResponse("Không tồn tại trọng số nào hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var ListratioRe = new List<RatioResponse>();
            foreach (var i in getById)
            {
                var groupNameOfVoter = await dbContext.Groups.Where(p => p.GroupId == i.GroupVoterId)
               .SingleOrDefaultAsync();
                var groupNameOfCandidate = await dbContext.Groups.Where(p => p.GroupId == i.GroupCandidateId)
               .SingleOrDefaultAsync();
                var ratioRe = new RatioResponse();

                ratioRe.RatioGroupId = i.RatioGroupId;
                ratioRe.Proportion = i.Proportion;
                ratioRe.GroupVoterId = i.GroupVoterId;
                ratioRe.GroupNameOfVoter = groupNameOfVoter.Name;
                ratioRe.GroupCandidateId = i.GroupCandidateId;
                ratioRe.GroupNameOfCandidate = groupNameOfCandidate.Name;
                ratioRe.CampaignId = i.CampaignId;
                ListratioRe.Add(ratioRe);

                ListratioRe = ListratioRe.OrderBy(p => p.GroupNameOfVoter).ToList();
            }
            response.Data = ListratioRe;
            response.ToSuccessResponse(response.Data, "Lấy danh sách tỷ trọng thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<RatioResponse>> UpdateRatio(Guid id, UpdateRatioRequest request)
        {
            APIResponse<RatioResponse> response = new();

            var ratio = await dbContext.Ratios.SingleOrDefaultAsync(c => c.RatioGroupId == id);
            if (ratio == null)
            {
                response.ToFailedResponse("Tỷ trọng không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == ratio.CampaignId);
            if (cam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (ratio.GroupVoterId != request.GroupVoterId)
            {
                response.ToFailedResponse("Nhóm của người bình chọn không đúng", StatusCodes.Status400BadRequest);
                return response;
            }
            if (ratio.GroupCandidateId != request.GroupCandidateId)
            {
                response.ToFailedResponse("Nhóm của người ứng cử không đúng", StatusCodes.Status400BadRequest);
                return response;
            }
            ratio.Proportion = request.Proportion;
            dbContext.Ratios.Update(ratio);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<RatioResponse>(cam);
            response.ToSuccessResponse("Cập nhật tỷ trọng thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }

}
