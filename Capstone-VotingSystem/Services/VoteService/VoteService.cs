using AutoMapper;
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
                response.ToFailedResponse("không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId);
            if (checkStateId == null)
            {
                response.ToFailedResponse("không tìm thấy giai đoạn", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCandidate = await dbContext.Candidates.SingleOrDefaultAsync(p => p.CandidateId == request.CandidateId && p.CampaignId == checkStateId.CampaignId);
            if (checkCandidate == null)
            {
                response.ToFailedResponse("không tìm thấy ứng cử viên hoặc ứng cử viên không thuộc chiến dịch này", StatusCodes.Status404NotFound);
                return response;
            }
            var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.CandidateId == request.CandidateId && p.StageId == request.StageId && p.Status == true);
            if (checkVote != null)
            {
                response.ToFailedResponse("Bạn đã bình chọn cho ứng cử viên này trong giai đoạn này rồi", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroupUser = await dbContext.GroupUsers.SingleOrDefaultAsync(p => p.UserId == checkUser.UserId && p.CampaignId == checkStateId.CampaignId);
            if (checkGroupUser == null)
            {
                response.ToFailedResponse("Bạn chưa chọn nhóm của mình khi tham gia chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupVoterId == checkGroupUser.GroupId && p.GroupCandidateId == checkCandidate.GroupId && p.CampaignId == checkStateId.CampaignId);
            if (ratioGroup == null)
            {
                response.ToFailedResponse("Tỷ trọng chưa được tạo", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkLimitVote = await dbContext.Votings.Where(p=>p.UserId==request.UserId&&p.StageId==request.StageId && p.Status==true).ToListAsync();
            if (checkLimitVote.Count >=checkStateId.LimitVote)
            {
                response.ToFailedResponse("Bạn đã hết phiếu để bình chọn cho giai đoạn này rồi", StatusCodes.Status400BadRequest);
                return response;
            }

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
            decimal scorevotedetail = 0;
            if (request.VotingDetail != null)
            {

                List<CreateVoteDetailResponse> listVotingDetail = new List<CreateVoteDetailResponse>();
                foreach (var i in request.VotingDetail)
                {
                    var scoreElement = await dbContext.Elements.Where(p => p.ElementId == i.ElementId).SingleOrDefaultAsync();
                    if (scoreElement != null)
                    {
                        scorevotedetail += scoreElement.Score;
                    }

                    var ide = Guid.NewGuid();
                    VotingDetail votingDetail = new VotingDetail();
                    {
                        votingDetail.VotingDetailId = ide;
                        votingDetail.CreateTime = DateTime.Now;
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
            var checkscore = await dbContext.Scores.Where(p => p.StageId == request.StageId && p.CandidateId == request.CandidateId).SingleOrDefaultAsync();
            double scoreVote = Decimal.ToDouble(scorevotedetail);
            if (checkscore == null)
            {
                var scoreid = Guid.NewGuid();
                
                Score sc = new();
                {
                    sc.Point = scoreVote * ratioGroup.Proportion;
                    sc.ScoreId = scoreid;
                    sc.CandidateId = request.CandidateId;
                    sc.StageId = request.StageId;
                }
                await dbContext.Scores.AddAsync(sc);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                checkscore.Point += scoreVote * ratioGroup.Proportion;
                dbContext.Scores.Update(checkscore);
                await dbContext.SaveChangesAsync();
            }
            response.ToSuccessResponse("Bình chọn thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> CreateVoteLike(CreateVoteLikeRequest request)
        {
            APIResponse<string> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == request.UserId).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId);
            if (checkStateId == null)
            {
                response.ToFailedResponse("không tìm thấy giai đoạn", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCandidate = await dbContext.Candidates.SingleOrDefaultAsync(p => p.CandidateId == request.CandidateId && p.CampaignId == checkStateId.CampaignId && p.Status == true);
            if (checkCandidate == null)
            {
                response.ToFailedResponse("không tìm thấy ứng cử viên hoặc ứng cử viên không thuộc chiến dịch này", StatusCodes.Status404NotFound);
                return response;
            }
            var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.CandidateId == request.CandidateId && p.StageId == request.StageId && p.Status == true);
            if (checkVote != null)
            {
                response.ToFailedResponse("Bạn đã bình chọn cho ứng cử viên này trong giai đoạn này rồi", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroupUser = await dbContext.GroupUsers.SingleOrDefaultAsync(p => p.UserId == checkUser.UserId && p.CampaignId == checkStateId.CampaignId);
            if (checkGroupUser == null)
            {
                response.ToFailedResponse("Bạn chưa chọn nhóm của mình khi tham gia chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupVoterId == checkGroupUser.GroupId && p.GroupCandidateId == checkCandidate.GroupId && p.CampaignId == checkStateId.CampaignId);
            if (ratioGroup == null)
            {
                response.ToFailedResponse("Tỷ trọng chưa được tạo", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkLimitVote = await dbContext.Votings.Where(p => p.UserId == request.UserId && p.StageId == request.StageId && p.Status == true).ToListAsync();
            if (checkLimitVote.Count >= checkStateId.LimitVote)
            {
                response.ToFailedResponse("Bạn đã hết phiếu để bình chọn cho giai đoạn này rồi", StatusCodes.Status400BadRequest);
                return response;
            }
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
            var checkscore = await dbContext.Scores.Where(p => p.StageId == request.StageId && p.CandidateId == request.CandidateId).SingleOrDefaultAsync();
            if (checkscore == null)
            {
                var scoreid = Guid.NewGuid();
                Score sc = new();
                {
                    sc.Point = ratioGroup.Proportion;
                    sc.ScoreId = scoreid;
                    sc.CandidateId = request.CandidateId;
                    sc.StageId = request.StageId;
                }
                await dbContext.Scores.AddAsync(sc);
                await dbContext.SaveChangesAsync();
            }
            else
            {
                checkscore.Point += ratioGroup.Proportion;
                dbContext.Scores.Update(checkscore);
                await dbContext.SaveChangesAsync();
            }
            await dbContext.Votings.AddAsync(vote);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Bình chọn thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
