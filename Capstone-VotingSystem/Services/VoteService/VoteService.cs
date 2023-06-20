using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.RequestModels.VotingDetailRequest;
using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;
using Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse;
using Capstone_VotingSystem.Models.ResponseModels.VotingResponse;
using Microsoft.EntityFrameworkCore;
using Octokit.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        public async Task<APIResponse<CreateVoteResponse>> CreateVote(CreateVoteRequest request)
        {
            APIResponse<CreateVoteResponse> response = new();
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
            var checkCandidate = await dbContext.Candidates.SingleOrDefaultAsync(p => p.CandidateId == request.CandidateId);
            if (checkCandidate == null)
            {
                response.ToSuccessResponse("không tìm thấy candidate", StatusCodes.Status404NotFound);
                return response;
            }
            var id = Guid.NewGuid();
            Voting vote = new Voting();
            {
                vote.VoringId = id;
                vote.UserId = request.UserId;
                vote.StageId = request.StageId;
                vote.RatioGroupId = request.RatioGroupId;
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
                        votingDetail.CreateTime = DateTime.Now;
                        votingDetail.ElementId = i.ElementId;
                        votingDetail.VotingId = vote.VoringId;
                    }
                    await dbContext.VotingDetails.AddAsync(votingDetail);
                    await dbContext.SaveChangesAsync();
                    var map1 = _mapper.Map<CreateVoteDetailResponse>(votingDetail);
                    listVotingDetail.Add(map1);

                }
                map.VoteDetails = listVotingDetail;
            }

            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<CreateVoteResponse>> CreateVotingDetail(Guid? votingId, CreateVotingDetailRequest request)
        {
            APIResponse<CreateVoteResponse> response = new();
            var checkVoting = await dbContext.Votings.SingleOrDefaultAsync(p => p.VoringId == votingId);
            if (checkVoting == null)
            {
                response.ToFailedResponse("không tìm thấy votingId", StatusCodes.Status404NotFound);
                return response;
            }
            var id = Guid.NewGuid();
            VotingDetail votingDetail = new VotingDetail();
            {
                votingDetail.VotingDetailId = id;
                votingDetail.CreateTime = DateTime.Now;
                votingDetail.ElementId = request.ElementId;
                votingDetail.VotingId = votingId;
            }
            await dbContext.AddAsync(votingDetail);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateVoteResponse>(checkVoting);
            map.Status = checkVoting.Status;
            var votingDetailList = await dbContext.VotingDetails.Where(p => p.VotingDetailId == votingId).ToListAsync();
            List<CreateVoteDetailResponse> listeVoteDetail = votingDetailList.Select(
           x =>
           {
               return new CreateVoteDetailResponse()
               {
                   VotingDetailId = x.VotingDetailId,
                   CreateTime = DateTime.Now,
                   ElementId = x.ElementId,
                   VotingId = x.VotingId,
               };
           }
           ).ToList();
            map.VoteDetails = listeVoteDetail;
            response.Data = map;
            response.ToSuccessResponse(response.Data, "Thêm thành công chi tiết", StatusCodes.Status200OK);
            return response;

        }
    }
}
