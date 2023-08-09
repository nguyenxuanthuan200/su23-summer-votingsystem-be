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
            var checkUser = await dbContext.Users.Where(p => p.UserId == request.UserId && p.Status == true).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId && p.Status == true);
            if (checkStateId == null)
            {
                response.ToFailedResponse("không tìm thấy giai đoạn", StatusCodes.Status404NotFound);
                return response;
            }
            if (!checkStateId.Process.Equals("Đang diễn ra"))
            {
                response.ToFailedResponse("Không thể bình chọn giai đoạn chưa diễn ra hoặc đã kết thúc", StatusCodes.Status400BadRequest);
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
            var checkLimitVote = await dbContext.Votings.Where(p => p.UserId == request.UserId && p.StageId == request.StageId && p.Status == true).ToListAsync();
            if (checkLimitVote.Count >= checkStateId.LimitVote)
            {
                response.ToFailedResponse("Bạn đã hết phiếu để bình chọn cho giai đoạn này rồi", StatusCodes.Status400BadRequest);
                return response;
            }
            Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
            if (ratioGroup.CampaignId == cam)
            {
                var check = await checkVoteSuccess(request.UserId, request.CandidateId, ratioGroup.CampaignId, request.StageId);
                if (check.Equals("false"))
                {
                    response.ToFailedResponse("Bạn không thể bình chọn cho giảng viên này do thể lệ của chiến dịch đề ra. Để biết thêm bạn vui lòng đọc thể lệ ở phía trên", StatusCodes.Status400BadRequest);
                    return response;
                }
            }
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var id = Guid.NewGuid();
            Voting vote = new Voting();
            {
                vote.VotingId = id;
                vote.UserId = request.UserId;
                vote.StageId = request.StageId;
                vote.RatioGroupId = ratioGroup.RatioGroupId;
                vote.CandidateId = request.CandidateId;
                vote.Status = true;
                vote.SendingTime = currentDateTimeVn;
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
                        votingDetail.CreateTime = currentDateTimeVn;
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
            //create history, notification
            var getTypeAction = await dbContext.TypeActions.SingleOrDefaultAsync(p => p.Name == "voted");
            var getCampaignName = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == checkStateId.CampaignId);
            var idHis = Guid.NewGuid();
            HistoryAction hisAc = new HistoryAction()
            {
                HistoryActionId = idHis,
                Description = "Đã bình chọn cho ứng viên " + checkCandidate.FullName + " trong chiến dịch - " + getCampaignName.Title,
                Time = currentDateTimeVn,
                TypeActionId = getTypeAction.TypeActionId,
                UserId = request.UserId,

            };
            await dbContext.HistoryActions.AddAsync(hisAc);
            await dbContext.SaveChangesAsync();

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
            if (!checkStateId.Process.Equals("Đang diễn ra"))
            {
                response.ToFailedResponse("Không thể bình chọn giai đoạn chưa diễn ra hoặc đã kết thúc", StatusCodes.Status400BadRequest);
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
            Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
            if (ratioGroup.CampaignId == cam)
            {
                var check = await checkVoteSuccess(request.UserId, request.CandidateId, ratioGroup.CampaignId, request.StageId);
                if (check.Equals("false"))
                {
                    response.ToFailedResponse("Bạn không thể bình chọn cho giảng viên này do thể lệ của chiến dịch đề ra. Để biết thêm bạn vui lòng đọc thể lệ ở phía trên", StatusCodes.Status400BadRequest);
                    return response;
                }
            }

            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var id = Guid.NewGuid();
            Voting vote = new Voting();
            {
                vote.VotingId = id;
                vote.UserId = request.UserId;
                vote.StageId = request.StageId;
                vote.RatioGroupId = ratioGroup.RatioGroupId;
                vote.CandidateId = request.CandidateId;
                vote.Status = true;
                vote.SendingTime = currentDateTimeVn;
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
            //create history, notification
            var getTypeAction = await dbContext.TypeActions.SingleOrDefaultAsync(p => p.Name == "voted");
            var getCampaignName = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == checkStateId.CampaignId);
            var idHis = Guid.NewGuid();
            HistoryAction hisAc = new HistoryAction()
            {
                HistoryActionId = idHis,
                Description = "Đã bình chọn cho ứng viên " + checkCandidate.FullName + " trong chiến dịch - " + getCampaignName.Title,
                Time = currentDateTimeVn,
                TypeActionId = getTypeAction.TypeActionId,
                UserId = request.UserId,

            };
            await dbContext.HistoryActions.AddAsync(hisAc);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Bình chọn thành công", StatusCodes.Status200OK);
            return response;
        }

        private async Task<string> checkVoteSuccess(string userId, Guid candidateId, Guid campaignId, Guid stageid)
        {
            string groupOfUser = "";
            var checkGroupUser = await dbContext.GroupUsers.Where(p => p.UserId == userId && p.CampaignId == campaignId).ToListAsync();
            foreach (var i in checkGroupUser)
            {
                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == i.GroupId && p.CampaignId == campaignId && p.IsVoter == true).SingleOrDefaultAsync();
                groupOfUser = checkGroup.Name;
            }
            string groupOfCandidate;
            var checkCandidate = await dbContext.Candidates.Where(p => p.CandidateId == candidateId && p.CampaignId == campaignId && p.Status == true).SingleOrDefaultAsync();
            if (checkCandidate == null)
            {
                return "Ứng cử viên này không thuộc chiến dịch này hoặc đã bị loại bỏ khỏi chiến dịch";
            }
            var checkGroupCandidate = await dbContext.Groups.Where(p => p.GroupId == checkCandidate.GroupId && p.CampaignId == campaignId && p.IsVoter == false).SingleOrDefaultAsync();
            if (checkGroupCandidate == null)
            {
                return "Nhóm của ứng cử viên này không thuộc chiến dịch này hoặc đã bị loại bỏ khỏi chiến dịch";
            }
            groupOfCandidate = checkGroupCandidate.Name;

            int groupCategoryOfCandidate = 0;

            if (groupOfCandidate.Equals("Nhạc cụ dân tộc") || groupOfCandidate.Equals("Tiếng anh dự bị") || groupOfCandidate.Equals("Giáo dục thể chất"))
                groupCategoryOfCandidate = 1;

            var listVoteOfUser = await dbContext.Votings.Where(p => p.UserId == userId && p.StageId == stageid && p.Status == true).ToListAsync();

            int countdb = 0;
            int countcn = 0;
            foreach (var vote in listVoteOfUser)
            {
                var checkCandidateOfVote = await dbContext.Candidates.Where(p => p.CandidateId == vote.CandidateId && p.CampaignId == campaignId && p.Status == true).SingleOrDefaultAsync();
                var checkGroupCandidateOfVote = await dbContext.Groups.Where(p => p.GroupId == checkCandidateOfVote.GroupId && p.CampaignId == campaignId && p.IsVoter == false).SingleOrDefaultAsync();
                if (checkGroupCandidateOfVote.Name.Equals("Nhạc cụ dân tộc") || checkGroupCandidateOfVote.Name.Equals("Tiếng anh dự bị") || checkGroupCandidateOfVote.Name.Equals("Giáo dục thể chất"))
                    countdb = countdb + 1;
                else
                    countcn = countcn + 1;

            }

            if (groupOfUser.Equals("Chuyên ngành 0") && groupCategoryOfCandidate == 1)
                return "success";
            if (groupOfUser.Equals("Chuyên ngành(1 - 6)") && groupCategoryOfCandidate == 1 && countdb == 0)
                return "success";
            if (groupOfUser.Equals("Chuyên ngành(1 - 6)") && groupCategoryOfCandidate == 0 && countcn <= 1)
                return "success";
            if (groupOfUser.Equals("Chuyên ngành(7 - 9)") && groupCategoryOfCandidate == 0 && countcn <= 2)
                return "success";

            return "false";

        }

        public async Task<APIResponse<IEnumerable<SatisticalVoteInCampaignResponse>>> StatisticalVoteByCampaign(StatisticalVoteRequest request)
        {
            APIResponse<IEnumerable<SatisticalVoteInCampaignResponse>> response = new();
            var result = new List<SatisticalVoteInCampaignResponse>();
            var SaInCam = new SatisticalVoteInCampaignResponse();

            var listSaInStage = new List<SatisticalVoteInStageResponse>();
            var SaInStage = new SatisticalVoteInStageResponse();

            var ListSaInGroup = new List<TotalVoteOfGroupInCampaignResponse>();
            var SaInGroup = new TotalVoteOfGroupInCampaignResponse();

            int TotalVoteInCampaign = 0;


            //if (request.StageId != Guid.Empty)
            //{
            var stage = await dbContext.Stages.Where(p => p.StageId == request.StageId && p.Status == true).SingleOrDefaultAsync();
            if (stage == null)
            {
                response.ToFailedResponse("Giai đoạn không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }

            while (request.DateAt.CompareTo(request.ToDate) <= 0)
            {
                var CountVoteByStage = await dbContext.Votings.Where(p => p.StageId == request.StageId && p.Status == true && p.SendingTime.Date.Equals(request.DateAt.Date)).ToListAsync();
                //CountVoteByStage = CountVoteByStage.Where(p => p.SendingTime.Date.Equals(request.DateAt.Date));
                ListSaInGroup = new();
                var listGroupVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == stage.CampaignId && p.IsVoter == true).ToListAsync();
                foreach (var group in listGroupVoterOfCampagin)
                {
                    SaInGroup = new TotalVoteOfGroupInCampaignResponse();
                    SaInGroup.GroupId = group.GroupId;
                    SaInGroup.GroupName = group.Name;
                    SaInGroup.TotalVote = 0;
                    ListSaInGroup.Add(SaInGroup);

                }

                foreach (var i in CountVoteByStage)
                {
                    var CountVoteInGroup = await dbContext.GroupUsers.Where(p => p.CampaignId == stage.CampaignId && p.UserId == i.UserId).ToListAsync();
                    foreach (var e in CountVoteInGroup)
                    {
                        var checkGroup = await dbContext.Groups.Where(p => p.GroupId == e.GroupId && p.IsVoter == true).SingleOrDefaultAsync();
                        if (checkGroup != null)
                        {
                            var index = ListSaInGroup.FindIndex(p => p is TotalVoteOfGroupInCampaignResponse && ((TotalVoteOfGroupInCampaignResponse)p).GroupId == checkGroup.GroupId);
                            if (index != -1)
                            {
                                ((TotalVoteOfGroupInCampaignResponse)ListSaInGroup[index]).TotalVote += 1;
                            }
                        }
                    }
                }
                SaInStage = new();
                SaInStage.StageId = stage.StageId;
                SaInStage.TotalVoteInStage = CountVoteByStage.Count();
                TotalVoteInCampaign = SaInStage.TotalVoteInStage;
                SaInStage.StageName = stage.Title;
                SaInStage.VoteOfGroup = ListSaInGroup;
                listSaInStage = new();
                listSaInStage.Add(SaInStage);
                SaInCam = new();
                SaInCam.VoteOfGroupInStage = listSaInStage;
                SaInCam.Date = request.DateAt;
                SaInCam.TotalVoteInCampaign = TotalVoteInCampaign;
                result.Add(SaInCam);


                request.DateAt = request.DateAt.AddDays(1);
            }

            //}
            //else
            //{
            //    var checkStage = await dbContext.Stages.Where(p => p.CampaignId == request.CampaignId && p.Status == true).ToListAsync();
            //    if (checkStage.Count == 0)
            //    {
            //        response.ToFailedResponse("Không thể thống kê chiến dịch không có giai đoạn nào", StatusCodes.Status404NotFound);
            //        return response;
            //    }
            //}






            response.ToSuccessResponse(response.Data = result, "Thống kê thành công", StatusCodes.Status200OK);
            return response;
        }

    }
}
