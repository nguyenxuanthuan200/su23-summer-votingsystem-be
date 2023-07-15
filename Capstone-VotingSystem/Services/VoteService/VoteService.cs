﻿using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.RequestModels.VotingDetailRequest;
using Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse;
using Capstone_VotingSystem.Models.ResponseModels.VotingResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.VoteService
{
    public class VoteService : IVoteService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public VoteService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
        }
        public async Task<APIResponse<string>> CreateVote(CreateVoteRequest request)
        {
            APIResponse<string> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == request.UserId).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("không tìm thấy user", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId);
            if (checkStateId == null)
            {
                response.ToSuccessResponse("không tìm thấy State", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCandidate = await dbContext.Candidates.SingleOrDefaultAsync(p => p.CandidateId == request.CandidateId && p.CampaignId == checkStateId.CampaignId);
            if (checkCandidate == null)
            {
                response.ToSuccessResponse("không tìm thấy candidate hoặc candidate không thuộc campaign này", StatusCodes.Status404NotFound);
                return response;
            }
            var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.CandidateId == request.CandidateId && p.StageId == request.StageId && p.Status == true);
            if (checkVote != null)
            {
                response.ToSuccessResponse("Bạn đã bình chọn cho ứng cử viên này trong giai đoạn này rồi", StatusCodes.Status404NotFound);
                return response;
            }
            var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupId == checkUser.GroupId && p.GroupCandidateId == checkCandidate.GroupCandidateId && p.CampaignId == checkStateId.CampaignId);

            var id = Guid.NewGuid();
            Voting vote = new Voting();
            {
                vote.VotingId = id;
                vote.UserId = request.UserId;
                vote.StageId = request.StageId;
                vote.RatioGroupId = ratioGroup.RatioGroupId;
                vote.CandidateId = request.CandidateId;
                vote.Status = true;
                vote.SendingTime = request.SendingTime;
            }
            await dbContext.Votings.AddAsync(vote);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateVoteResponse>(vote);
            if (request.VotingDetail != null)
            {
                List<CreateVoteDetailResponse> listVotingDetail = new List<CreateVoteDetailResponse>();
                foreach (var i in request.VotingDetail)
                {
                    var ide = Guid.NewGuid();
                    VotingDetail votingDetail = new VotingDetail();
                    {
                        votingDetail.VotingDetailId = ide;
                        votingDetail.Time = DateTime.Now;
                        votingDetail.ElementId = i.ElementId;
                        votingDetail.VotingId = vote.VotingId;
                    }
                    await dbContext.VotingDetails.AddAsync(votingDetail);
                    await dbContext.SaveChangesAsync();
                    var map1 = _mapper.Map<CreateVoteDetailResponse>(votingDetail);
                    listVotingDetail.Add(map1);
                }
                map.VoteDetails = listVotingDetail;
            }
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            //response.Data = map;
            return response;
        }

        public async Task<APIResponse<string>> CreateVoteLike(CreateVoteLikeRequest request)
        {
            APIResponse<string> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == request.UserId).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("không tìm thấy user", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId);
            if (checkStateId == null)
            {
                response.ToSuccessResponse("không tìm thấy State", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCandidate = await dbContext.Candidates.SingleOrDefaultAsync(p => p.CandidateId == request.CandidateId && p.CampaignId == checkStateId.CampaignId);
            if (checkCandidate == null)
            {
                response.ToSuccessResponse("không tìm thấy candidate hoặc candidate không thuộc campaign này", StatusCodes.Status404NotFound);
                return response;
            }
            var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.CandidateId == request.CandidateId && p.StageId == request.StageId && p.Status==true);
            if (checkVote != null)
            {
                response.ToSuccessResponse("Bạn đã bình chọn cho ứng cử viên này trong giai đoạn này rồi", StatusCodes.Status404NotFound);
                return response;
            }
            var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupId == checkUser.GroupId && p.GroupCandidateId == checkCandidate.GroupCandidateId && p.CampaignId== checkStateId.CampaignId);
           
            var id = Guid.NewGuid();
            Voting vote = new Voting();
            {
                vote.VotingId = id;
                vote.UserId = request.UserId;
                vote.StageId = request.StageId;
                vote.RatioGroupId = ratioGroup.RatioGroupId;
                vote.CandidateId = request.CandidateId;
                vote.Status = true;
                vote.SendingTime = request.SendingTime;
            }
            await dbContext.Votings.AddAsync(vote);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
