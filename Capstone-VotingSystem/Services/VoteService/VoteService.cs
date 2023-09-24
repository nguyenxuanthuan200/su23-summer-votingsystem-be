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
            var checkUser = await dbContext.Users.Where(p => p.UserId == request.UserId && p.Status == true).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId && p.Status == true);
            if (checkStateId == null)
            {
                response.ToFailedResponse("Không tìm thấy giai đoạn", StatusCodes.Status404NotFound);
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
                response.ToFailedResponse("Không tìm thấy ứng cử viên hoặc ứng cử viên không thuộc chiến dịch này", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroupUser = await dbContext.GroupUsers.Where(p => p.UserId == checkUser.UserId && p.CampaignId == checkStateId.CampaignId).ToListAsync();
            var GroupUser = new Group();
            int n = 0;
            foreach (var item in checkGroupUser)
            {
                var group = await dbContext.Groups.Where(p => p.GroupId == item.GroupId && p.IsVoter == true && p.IsStudentMajor == false).SingleOrDefaultAsync();
                if (group != null)
                {
                    GroupUser = group;
                    n = 1;
                }

            }
            if (n == 0)
            {
                response.ToFailedResponse("Bạn chưa chọn nhóm của mình khi tham gia chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.CandidateId == request.CandidateId && p.StageId == request.StageId && p.Status == true);
            if (checkVote != null)
            {
                checkVote.Status = false;

                //-score
                var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.RatioGroupId == checkVote.RatioGroupId);

                var getVoteDetail = await dbContext.VotingDetails.Where(p => p.VotingId == checkVote.VotingId).ToListAsync();
                decimal scorevotedetail = 0;
                foreach (var vote in getVoteDetail)
                {
                    var scoreElement = await dbContext.Elements.Where(p => p.ElementId == vote.ElementId && p.Status == true).SingleOrDefaultAsync();
                    if (scoreElement != null)
                    {
                        scorevotedetail += scoreElement.Score;
                    }
                    else
                    {
                        response.ToFailedResponse("Có lỗi liên quan đến câu trả lời nên không thể bỏ bình chọn", StatusCodes.Status400BadRequest);
                        return response;
                    }
                }
                double scoreVote = Decimal.ToDouble(scorevotedetail);
                var checkscore = await dbContext.Scores.Where(p => p.StageId == request.StageId && p.CandidateId == request.CandidateId).SingleOrDefaultAsync();
                checkscore.Point = checkscore.Point - (ratioGroup.Proportion * scoreVote);

                var getTypeAction = await dbContext.TypeActions.SingleOrDefaultAsync(p => p.Name == "edit vote");
                var getCampaignName = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == checkStateId.CampaignId);
                var idHis = Guid.NewGuid();
                HistoryAction hisAc = new HistoryAction()
                {
                    HistoryActionId = idHis,
                    Description = "Đã bỏ bình chọn cho " + checkCandidate.FullName + " trong chiến dịch - " + getCampaignName.Title,
                    Time = currentDateTimeVn,
                    TypeActionId = getTypeAction.TypeActionId,
                    UserId = request.UserId,

                };

                await dbContext.HistoryActions.AddAsync(hisAc);
                dbContext.Votings.Update(checkVote);
                dbContext.Scores.Update(checkscore);
                await dbContext.SaveChangesAsync();

                response.ToSuccessResponse("Bỏ bình chọn thành công.", StatusCodes.Status200OK);
                return response;
            }
            else
            {


                var checkLimitVote = await dbContext.Votings.Where(p => p.UserId == request.UserId && p.StageId == request.StageId && p.Status == true).ToListAsync();
                if (checkLimitVote.Count >= checkStateId.LimitVote)
                {
                    response.ToFailedResponse("Bạn đã hết phiếu để bình chọn cho giai đoạn này rồi", StatusCodes.Status400BadRequest);
                    return response;
                }
                int remainingVotes = 0;
                remainingVotes = checkStateId.LimitVote - checkLimitVote.Count - 1;
                Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
                if (checkStateId.CampaignId == cam)
                {
                    var check = await checkVoteSuccess(request.UserId, request.CandidateId, checkStateId.CampaignId, request.StageId);
                    if (check.Equals("false"))
                    {
                        response.ToFailedResponse("Bạn không thể bình chọn cho giảng viên này do thể lệ của chiến dịch đề ra.", StatusCodes.Status400BadRequest);
                        return response;
                    }
                }
                var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupVoterId == GroupUser.GroupId && p.GroupCandidateId == checkCandidate.GroupId && p.CampaignId == checkStateId.CampaignId);

                if (ratioGroup == null)
                {
                    response.ToFailedResponse("Tỷ trọng chưa được tạo", StatusCodes.Status400BadRequest);
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

                response.ToSuccessResponse("Bình chọn thành công. Bạn còn " + remainingVotes + " phiếu còn lại", StatusCodes.Status200OK);
                return response;
            }
        }

        public async Task<APIResponse<string>> CreateVoteLike(CreateVoteLikeRequest request)
        {
            APIResponse<string> response = new();
            var checkUser = await dbContext.Users.Where(p => p.UserId == request.UserId && p.Status == true).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Không tìm thấy người dùng", StatusCodes.Status404NotFound);
                return response;
            }
            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == request.StageId && p.Status == true);
            if (checkStateId == null)
            {
                response.ToFailedResponse("Không tìm thấy giai đoạn", StatusCodes.Status404NotFound);
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
            //  var groupNames = dbContext.GroupUsers.Where(u => u.UserId == checkUser.UserId && checkStateId.CampaignId == checkStateId.CampaignId).Join(dbContext.Groups, u => u.GroupId, ug => ug.GroupId, (u, ug) => ug.GroupId)
            //.Join(dbContext.Groups, gid => gid, g => g.GroupId, (gid, g) => g.Name);
            var checkGroupUser = await dbContext.GroupUsers.Where(p => p.UserId == checkUser.UserId && p.CampaignId == checkStateId.CampaignId).ToListAsync();
            var GroupUser = new Group();
            int n = 0;
            foreach (var item in checkGroupUser)
            {
                var group = await dbContext.Groups.Where(p => p.GroupId == item.GroupId && p.IsVoter == true && p.IsStudentMajor == false).SingleOrDefaultAsync();
                if (group != null)
                {
                    GroupUser = group;
                    n = 1;
                }

            }
            if (n == 0)
            {
                response.ToFailedResponse("Bạn chưa chọn nhóm của mình khi tham gia chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            //getTimeNow()
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
            //
            var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == request.UserId && p.CandidateId == request.CandidateId && p.StageId == request.StageId && p.Status == true);
            if (checkVote != null)
            {
                checkVote.Status = false;

                //-score
                var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.RatioGroupId == checkVote.RatioGroupId);

                var checkscore = await dbContext.Scores.Where(p => p.StageId == request.StageId && p.CandidateId == request.CandidateId).SingleOrDefaultAsync();
                checkscore.Point = checkscore.Point - ratioGroup.Proportion;

                var getTypeAction = await dbContext.TypeActions.SingleOrDefaultAsync(p => p.Name == "edit vote");
                var getCampaignName = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == checkStateId.CampaignId);
                var idHis = Guid.NewGuid();
                HistoryAction hisAc = new HistoryAction()
                {
                    HistoryActionId = idHis,
                    Description = "Đã bỏ bình chọn cho " + checkCandidate.FullName + " trong chiến dịch - " + getCampaignName.Title,
                    Time = currentDateTimeVn,
                    TypeActionId = getTypeAction.TypeActionId,
                    UserId = request.UserId,

                };

                await dbContext.HistoryActions.AddAsync(hisAc);
                dbContext.Votings.Update(checkVote);
                dbContext.Scores.Update(checkscore);
                await dbContext.SaveChangesAsync();

                response.ToSuccessResponse("Bỏ bình chọn thành công.", StatusCodes.Status200OK);
                return response;
            }
            else
            {


                var checkLimitVote = await dbContext.Votings.Where(p => p.UserId == request.UserId && p.StageId == request.StageId && p.Status == true).ToListAsync();
                if (checkLimitVote.Count >= checkStateId.LimitVote)
                {
                    response.ToFailedResponse("Bạn đã hết phiếu để bình chọn cho giai đoạn này rồi", StatusCodes.Status400BadRequest);
                    return response;
                }
                int remainingVotes = 0;
                remainingVotes = checkStateId.LimitVote - checkLimitVote.Count - 1;
                Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
                if (checkStateId.CampaignId == cam)
                {
                    var check = await checkVoteSuccess(request.UserId, request.CandidateId, checkStateId.CampaignId, request.StageId);
                    if (check.Equals("false"))
                    {
                        response.ToFailedResponse("Bạn không thể bình chọn cho giảng viên này do thể lệ của chiến dịch đề ra.", StatusCodes.Status400BadRequest);
                        return response;
                    }
                }
                var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupVoterId == GroupUser.GroupId && p.GroupCandidateId == checkCandidate.GroupId && p.CampaignId == checkStateId.CampaignId);

                if (ratioGroup == null)
                {
                    response.ToFailedResponse("Tỷ trọng chưa được tạo", StatusCodes.Status400BadRequest);
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
                response.ToSuccessResponse("Bình chọn thành công. Bạn còn " + remainingVotes + " phiếu còn lại", StatusCodes.Status200OK);
                return response;
            }
        }

        private async Task<string> checkVoteSuccess(string userId, Guid candidateId, Guid campaignId, Guid stageid)
        {
            string groupOfUser = "";
            var checkGroupUser = await dbContext.GroupUsers.Where(p => p.UserId == userId && p.CampaignId == campaignId).ToListAsync();
            foreach (var i in checkGroupUser)
            {
                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == i.GroupId && p.CampaignId == campaignId && p.IsVoter == true && p.IsStudentMajor == false).SingleOrDefaultAsync();
                if (checkGroup != null)
                    groupOfUser = checkGroup.GroupId.ToString();
            }
            Guid groupOfCandidate;
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
            groupOfCandidate = checkGroupCandidate.GroupId;

            int groupCategoryOfCandidate = 0;
            Guid db1 = Guid.Parse("566fa89f-5730-45cc-b97d-2842ba1199e7");
            Guid db2 = Guid.Parse("6101f9ff-55e1-4785-914f-216dadfbfae5");
            Guid db3 = Guid.Parse("98d60b6d-5c5e-4cdb-b289-be92ffc77206");
            Guid db4 = Guid.Parse("c5a820f6-1093-4355-80be-d814ae0dfad0");
            Guid db5 = Guid.Parse("d8111aba-574e-4c2f-837a-e9a1cbfd36d2");
            if (groupOfCandidate == db1 || groupOfCandidate == db2 || groupOfCandidate == db3 || groupOfCandidate == db4
                || groupOfCandidate == db5)
                groupCategoryOfCandidate = 1;

            var listVoteOfUser = await dbContext.Votings.Where(p => p.UserId == userId && p.StageId == stageid && p.Status == true).ToListAsync();

            int countdb = 0;
            int countcn = 0;
            foreach (var vote in listVoteOfUser)
            {
                var checkCandidateOfVote = await dbContext.Candidates.Where(p => p.CandidateId == vote.CandidateId && p.CampaignId == campaignId && p.Status == true).SingleOrDefaultAsync();
                var checkGroupCandidateOfVote = await dbContext.Groups.Where(p => p.GroupId == checkCandidateOfVote.GroupId && p.CampaignId == campaignId && p.IsVoter == false).SingleOrDefaultAsync();
                if (checkGroupCandidateOfVote.GroupId == db1 || checkGroupCandidateOfVote.GroupId == db2
                    || checkGroupCandidateOfVote.GroupId == db3 || checkGroupCandidateOfVote.GroupId == db4
                    || checkGroupCandidateOfVote.GroupId == db5)
                    countdb = countdb + 1;
                else
                    countcn = countcn + 1;

            }

            if (groupOfUser.Equals("647be514-4a6a-4298-991d-912af5d16921") && groupCategoryOfCandidate == 1)
                return "success";
            if (groupOfUser.Equals("8307dd09-2299-49b4-85ad-5aba6e5c474a") && groupCategoryOfCandidate == 1 && countdb == 0)
                return "success";
            if (groupOfUser.Equals("8307dd09-2299-49b4-85ad-5aba6e5c474a") && groupCategoryOfCandidate == 0 && countcn <= 1)
                return "success";
            if (groupOfUser.Equals("04fa2169-155c-4b42-8a9a-48aa3336d461") && groupCategoryOfCandidate == 0 && countcn <= 2)
                return "success";

            return "false";

        }

        //public async Task<APIResponse<IEnumerable<SatisticalVoteInCampaignResponse>>> StatisticalVoteByCampaign(StatisticalVoteRequest request)
        //{
        //    APIResponse<IEnumerable<SatisticalVoteInCampaignResponse>> response = new();
        //    var result = new List<SatisticalVoteInCampaignResponse>();
        //    var SaInCam = new SatisticalVoteInCampaignResponse();

        //    var listSaInStage = new List<SatisticalVoteInStageResponse>();
        //    var SaInStage = new SatisticalVoteInStageResponse();

        //    var ListSaInGroup = new List<TotalVoteOfGroupInCampaignResponse>();
        //    var SaInGroup = new TotalVoteOfGroupInCampaignResponse();

        //    var GroupMajor = new TotalVoteOfGroupMajorResponse();
        //    var ListGroupMajor = new List<TotalVoteOfGroupMajorResponse>();
        //    int TotalVoteInCampaign = 0;

        //    var checkCampaign = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId && p.Status == true).SingleOrDefaultAsync();
        //    if (checkCampaign == null)
        //    {
        //        response.ToFailedResponse("Không thể thống kê chiến dịch không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    if (checkCampaign.StartTime.CompareTo(request.DateAt) > 0)
        //    {
        //        response.ToFailedResponse("Không thể thống kê chiến dịch vào thời điểm chưa bắt đầu", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    if (request.DateAt.CompareTo(request.ToDate) > 0)
        //    {
        //        response.ToFailedResponse("Không thể thống kê chiến dịch vì đến ngày trước từ ngày", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    var GetListStage = await dbContext.Stages.Where(p => p.CampaignId == request.CampaignId && p.Status == true).ToListAsync();
        //    if (GetListStage.Count <= 0)
        //    {
        //        response.ToFailedResponse("Không thể thống kê chiến dịch không có giai đoạn nào", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    else
        //    {

        //        //var stage = await dbContext.Stages.Where(p => p.StageId == item.StageId && p.Status == true).SingleOrDefaultAsync();
        //        //if (stage == null)
        //        //{
        //        //    response.ToFailedResponse("Có lỗi khi lấy danh sách giai đoạn của chiến dịch ", StatusCodes.Status400BadRequest);
        //        //    return response;
        //        //}
        //        //listSaInStage = new();
        //        while (request.DateAt.CompareTo(request.ToDate) <= 0)
        //        {
        //            foreach (var item in GetListStage)
        //            {

        //                //CountVoteByStage = CountVoteByStage.Where(p => p.SendingTime.Date.Equals(request.DateAt.Date));
        //                ListSaInGroup = new();
        //                var listGroupVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == item.CampaignId && p.IsVoter == true && p.IsStudentMajor == false).ToListAsync();
        //                foreach (var group in listGroupVoterOfCampagin)
        //                {
        //                    SaInGroup = new TotalVoteOfGroupInCampaignResponse();
        //                    SaInGroup.GroupId = group.GroupId;
        //                    SaInGroup.GroupName = group.Name;
        //                    SaInGroup.TotalVote = 0;
        //                    ListSaInGroup.Add(SaInGroup);

        //                }
        //                ListGroupMajor = new();
        //                var listGroupMajorVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == item.CampaignId && p.IsVoter == true && p.IsStudentMajor == true).ToListAsync();
        //                foreach (var group in listGroupMajorVoterOfCampagin)
        //                {
        //                    GroupMajor = new TotalVoteOfGroupMajorResponse();
        //                    GroupMajor.GroupId = group.GroupId;
        //                    GroupMajor.GroupName = group.Name;
        //                    GroupMajor.TotalVote = 0;
        //                    ListGroupMajor.Add(GroupMajor);

        //                }
        //                var CountVoteByStage = await dbContext.Votings.Where(p => p.StageId == item.StageId && p.Status == true && p.SendingTime.Date.Equals(request.DateAt.Date)).ToListAsync();
        //                CountVoteByStage = CountVoteByStage.DistinctBy(x => x.UserId).ToList();
        //                foreach (var i in CountVoteByStage)
        //                {
        //                    var CountVoteInGroup = await dbContext.GroupUsers.Where(p => p.CampaignId == item.CampaignId && p.UserId == i.UserId).ToListAsync();
        //                    foreach (var e in CountVoteInGroup)
        //                    {
        //                        var checkGroup = await dbContext.Groups.Where(p => p.GroupId == e.GroupId && p.IsVoter == true).SingleOrDefaultAsync();
        //                        if (checkGroup != null && checkGroup.IsStudentMajor == false)
        //                        {
        //                            var index = ListSaInGroup.FindIndex(p => p is TotalVoteOfGroupInCampaignResponse && ((TotalVoteOfGroupInCampaignResponse)p).GroupId == checkGroup.GroupId);
        //                            if (index != -1)
        //                            {
        //                                ((TotalVoteOfGroupInCampaignResponse)ListSaInGroup[index]).TotalVote += 1;
        //                            }
        //                        }
        //                        if (checkGroup != null && checkGroup.IsStudentMajor == true)
        //                        {
        //                            var index = ListGroupMajor.FindIndex(p => p is TotalVoteOfGroupMajorResponse && ((TotalVoteOfGroupMajorResponse)p).GroupId == checkGroup.GroupId);
        //                            if (index != -1)
        //                            {
        //                                ((TotalVoteOfGroupMajorResponse)ListGroupMajor[index]).TotalVote += 1;
        //                            }
        //                        }
        //                    }
        //                }
        //                SaInStage = new();
        //                SaInStage.StageId = item.StageId;
        //                SaInStage.TotalVoteInStage = CountVoteByStage.Count();
        //                TotalVoteInCampaign = SaInStage.TotalVoteInStage;
        //                SaInStage.StageName = item.Title;
        //                SaInStage.VoteOfGroup = ListSaInGroup;
        //                SaInStage.VoteOfGroupMajor = ListGroupMajor;

        //                listSaInStage.Add(SaInStage);

        //            }
        //            SaInCam = new();
        //            SaInCam.VoteOfGroupInStage = listSaInStage;
        //            listSaInStage = new();
        //            SaInCam.Date = request.DateAt.ToString("dd/MM/yyyy");
        //            SaInCam.TotalVoteInCampaign = TotalVoteInCampaign;
        //            result.Add(SaInCam);

        //            request.DateAt = request.DateAt.AddDays(1);
        //        }




        //    }



        //    response.ToSuccessResponse(response.Data = result, "Thống kê thành công", StatusCodes.Status200OK);
        //    return response;
        //}

        public async Task<APIResponse<SatisticalVoteInCampaignResponse>> StatisticalVoteByCampaign(StatisticalVoteRequest request)
        {
            APIResponse<SatisticalVoteInCampaignResponse> response = new();
            var SaInCam = new SatisticalVoteInCampaignResponse();

            var listSaInStage = new List<SatisticalVoteInStageResponse>();
            var SaInStage = new SatisticalVoteInStageResponse();

            var ListSaInGroup = new List<TotalVoteOfGroupInCampaignResponse>();
            var SaInGroup = new TotalVoteOfGroupInCampaignResponse();

            var GroupMajor = new TotalVoteOfGroupMajorResponse();
            var ListGroupMajor = new List<TotalVoteOfGroupMajorResponse>();
            int TotalVoteInCampaign = 0;
            int TotalVoteInCampaignByFilter = 0;

            var checkCampaign = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId && p.Status == true).SingleOrDefaultAsync();
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Không thể thống kê chiến dịch không tồn tại hoặc bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkCampaign.StartTime.CompareTo(request.DateAt) > 0)
            {
                response.ToFailedResponse("Không thể thống kê chiến dịch vào thời điểm chưa bắt đầu", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.DateAt.CompareTo(request.ToDate) > 0)
            {
                response.ToFailedResponse("Không thể thống kê chiến dịch vì đến ngày trước từ ngày", StatusCodes.Status400BadRequest);
                return response;
            }
            var GetListStage = await dbContext.Stages.Where(p => p.CampaignId == request.CampaignId && p.Status == true).ToListAsync();
            if (GetListStage.Count <= 0)
            {
                response.ToFailedResponse("Không thể thống kê chiến dịch không có giai đoạn nào", StatusCodes.Status400BadRequest);
                return response;
            }
            else
            {
                ListSaInGroup = new();
                var listGroupVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == request.CampaignId && p.IsVoter == true && p.IsStudentMajor == false).ToListAsync();
                foreach (var group in listGroupVoterOfCampagin)
                {
                    SaInGroup = new TotalVoteOfGroupInCampaignResponse();
                    SaInGroup.GroupId = group.GroupId;
                    SaInGroup.GroupName = group.Name;
                    SaInGroup.TotalVote = 0;
                    ListSaInGroup.Add(SaInGroup);

                }
                ListGroupMajor = new();
                var listGroupMajorVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == request.CampaignId && p.IsVoter == true && p.IsStudentMajor == true).ToListAsync();
                foreach (var group in listGroupMajorVoterOfCampagin)
                {
                    GroupMajor = new TotalVoteOfGroupMajorResponse();
                    GroupMajor.GroupId = group.GroupId;
                    GroupMajor.GroupName = group.Name;
                    GroupMajor.TotalVote = 0;
                    ListGroupMajor.Add(GroupMajor);

                }

                foreach (var item in GetListStage)
                {

                    var CountTotalVoteInCampaign = await dbContext.Votings.Where(p => p.StageId == item.StageId && p.Status == true).ToListAsync();
                    CountTotalVoteInCampaign = CountTotalVoteInCampaign.DistinctBy(x => x.UserId).ToList();
                    TotalVoteInCampaign += CountTotalVoteInCampaign.Count();

                    var totalVoteInStage = 0;
                    while (request.DateAt.CompareTo(request.ToDate) <= 0)
                    {
                        var CountVoteByStage = await dbContext.Votings.Where(p => p.StageId == item.StageId && p.Status == true && p.SendingTime.Date.Equals(request.DateAt.Date)).ToListAsync();
                        CountVoteByStage = CountVoteByStage.DistinctBy(x => x.UserId).ToList();
                        foreach (var i in CountVoteByStage)
                        {
                            var CountVoteInGroup = await dbContext.GroupUsers.Where(p => p.CampaignId == item.CampaignId && p.UserId == i.UserId).ToListAsync();
                            foreach (var e in CountVoteInGroup)
                            {
                                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == e.GroupId && p.IsVoter == true).SingleOrDefaultAsync();
                                if (checkGroup != null && checkGroup.IsStudentMajor == false)
                                {
                                    var index = ListSaInGroup.FindIndex(p => p is TotalVoteOfGroupInCampaignResponse && ((TotalVoteOfGroupInCampaignResponse)p).GroupId == checkGroup.GroupId);
                                    if (index != -1)
                                    {
                                        ((TotalVoteOfGroupInCampaignResponse)ListSaInGroup[index]).TotalVote += 1;
                                    }
                                }
                                if (checkGroup != null && checkGroup.IsStudentMajor == true)
                                {
                                    var index = ListGroupMajor.FindIndex(p => p is TotalVoteOfGroupMajorResponse && ((TotalVoteOfGroupMajorResponse)p).GroupId == checkGroup.GroupId);
                                    if (index != -1)
                                    {
                                        ((TotalVoteOfGroupMajorResponse)ListGroupMajor[index]).TotalVote += 1;
                                    }
                                }
                            }
                        }
                        totalVoteInStage += CountVoteByStage.Count();




                        request.DateAt = request.DateAt.AddDays(1);

                    }
                    SaInStage = new();
                    SaInStage.StageId = item.StageId;
                    SaInStage.TotalVoteInStage = totalVoteInStage;
                    SaInStage.StageName = item.Title;
                    SaInStage.VoteOfGroup = ListSaInGroup;
                    SaInStage.VoteOfGroupMajor = ListGroupMajor;
                    listSaInStage.Add(SaInStage);
                    TotalVoteInCampaignByFilter += SaInStage.TotalVoteInStage;

                }
            }
            SaInCam = new();
            SaInCam.VoteOfGroupInStage = listSaInStage;
            SaInCam.TotalVoteInCampaignByFilter = TotalVoteInCampaignByFilter;
            SaInCam.TotalVoteInCampaign = TotalVoteInCampaign;



            response.ToSuccessResponse(response.Data = SaInCam, "Thống kê thành công", StatusCodes.Status200OK);
            return response;
        }


    }
}
