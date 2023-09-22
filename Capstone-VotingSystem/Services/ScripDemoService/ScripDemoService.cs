using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.ScripDemoService
{
    public class ScripDemoService : IScripDemoService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;
        public ScripDemoService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<string>> ScripEndCampaign(Guid CampaignId)
        {
            APIResponse<string> response = new();
            var campaign = await dbContext.Campaigns.Where(p => p.CampaignId == CampaignId && p.Status == true).SingleOrDefaultAsync();

            var listStage = await dbContext.Stages.Where(p => p.CampaignId == campaign.CampaignId && p.Status == true).ToListAsync();
            if (listStage.Count > 0)
            {
                foreach (var stage in listStage)
                {
                    if (stage.Process == "Chưa bắt đầu")
                    {
                            stage.Process = "Đã kết thúc";
                        dbContext.Stages.Update(stage);

                    }
                    else if (stage.Process == "Đang diễn ra")
                    {
                            stage.Process = "Đã kết thúc";
                            dbContext.Stages.Update(stage);
                    }
                }
                if (campaign.Process == "Chưa bắt đầu")
                {
                        campaign.Process = "Đã kết thúc";
                        dbContext.Campaigns.Update(campaign);
              
                }
                else if (campaign.Process == "Đang diễn ra")
                {
                
                        campaign.Process = "Đã kết thúc";
                        dbContext.Campaigns.Update(campaign);
              
                }
            }
            else
            {
                response.ToFailedResponse("Chạy scrip kết thúc chiến dịch không thành công vì chiến dịch không có giai đoạn nào", StatusCodes.Status400BadRequest);
                return response;

            }
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Chạy scrip kết thúc chiến dịch thành công", StatusCodes.Status200OK);
            return response;
        }

        public Task<APIResponse<string>> ScripUnVote(Guid CampaignId, Guid StageId)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse<string>> ScripVote(Guid CampaignId, Guid StageId)
        {
            APIResponse<string> response = new();
            var checkCampaign = await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == CampaignId && p.Status == true && p.IsApprove == true);
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Không tìm thấy chiến dịch hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            if (!checkCampaign.Process.Equals("Đang diễn ra"))
            {
                response.ToFailedResponse("Không thể bình chọn chiến dịch chưa diễn ra hoặc đã kết thúc", StatusCodes.Status404NotFound);
                return response;
            }

            var checkStateId = await dbContext.Stages.SingleOrDefaultAsync(p => p.StageId == StageId && p.Status == true);
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
            //getTimeNow()
            TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);

            for (var i = 1; i <= 10; i++)
            {
                string userId = "scripaccount" + i;

                var checkGroupUser = await dbContext.GroupUsers.Where(p => p.UserId == userId && p.CampaignId == checkStateId.CampaignId).ToListAsync();
                var GroupUser = new Group();
               
                foreach (var item in checkGroupUser)
                {
                    var group = await dbContext.Groups.Where(p => p.GroupId == item.GroupId && p.IsVoter == true && p.IsStudentMajor == false).SingleOrDefaultAsync();
                    if (group != null)
                    {
                        GroupUser = group;
                    }
                }
                if (checkGroupUser.Count == 0)
                {
                    var getRandomGroup = await dbContext.Groups.Where(p => p.CampaignId == CampaignId && p.IsVoter == true && p.IsStudentMajor == false).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var id = Guid.NewGuid();
                    GroupUser groupUser = new GroupUser();
                    {
                        groupUser.GroupUserId = id;
                        groupUser.GroupId = getRandomGroup.GroupId;
                        groupUser.UserId = userId;
                        groupUser.CampaignId = CampaignId;
                    }
                    await dbContext.GroupUsers.AddAsync(groupUser);
                    await dbContext.SaveChangesAsync();

                    GroupUser = getRandomGroup;
                }
                for (var o = 0; o <= 9; o++)
                {
                    var getRandomCandidate = await dbContext.Candidates.Where(p => p.CampaignId == CampaignId && p.Status == true).OrderBy(x => Guid.NewGuid()).FirstOrDefaultAsync();
                    var checkVote = await dbContext.Votings.SingleOrDefaultAsync(p => p.UserId == userId && p.CandidateId == getRandomCandidate.CandidateId && p.StageId == StageId && p.Status == true);
                    if (checkVote != null)
                    {
                        //checkVote.Status = false;

                        ////-score
                        //var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.RatioGroupId == checkVote.RatioGroupId);

                        //var checkscore = await dbContext.Scores.Where(p => p.StageId == StageId && p.CandidateId == getRandomCandidate.CandidateId).SingleOrDefaultAsync();
                        //checkscore.Point = checkscore.Point - ratioGroup.Proportion;

                        //dbContext.Votings.Update(checkVote);
                        //dbContext.Scores.Update(checkscore);
                        //await dbContext.SaveChangesAsync();

                        //response.ToSuccessResponse("Bỏ bình chọn thành công.", StatusCodes.Status200OK);
                        //return response;
                    }
                    else
                    {
                        var checkLimitVote = await dbContext.Votings.Where(p => p.UserId == userId && p.StageId == StageId && p.Status == true).ToListAsync();
                        if (checkLimitVote.Count >= checkStateId.LimitVote)
                        {
                            break;
                        }
                        Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
                        if (checkStateId.CampaignId == cam)
                        {
                            var check = await checkVoteSuccess(userId, getRandomCandidate.CandidateId, CampaignId, StageId);
                            if (check.Equals("false"))
                            {
                                continue;
                            }
                        }
                        var ratioGroup = await dbContext.Ratios.SingleOrDefaultAsync(p => p.GroupVoterId == GroupUser.GroupId && p.GroupCandidateId == getRandomCandidate.GroupId && p.CampaignId == checkStateId.CampaignId);

                        var id = Guid.NewGuid();
                        Voting vote = new Voting();
                        {
                            vote.VotingId = id;
                            vote.UserId = userId;
                            vote.StageId = StageId;
                            vote.RatioGroupId = ratioGroup.RatioGroupId;
                            vote.CandidateId = getRandomCandidate.CandidateId;
                            vote.Status = true;
                            vote.SendingTime = currentDateTimeVn;
                        }
                        var checkscore = await dbContext.Scores.Where(p => p.StageId == StageId && p.CandidateId == getRandomCandidate.CandidateId).SingleOrDefaultAsync();
                        if (checkscore == null)
                        {
                            var scoreid = Guid.NewGuid();
                            Score sc = new();
                            {
                                sc.Point = ratioGroup.Proportion;
                                sc.ScoreId = scoreid;
                                sc.CandidateId = getRandomCandidate.CandidateId;
                                sc.StageId = StageId;
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
                    }
                }
            }
            response.ToSuccessResponse("Chạy scrip thành công", StatusCodes.Status200OK);
            return response;


        }
        private async Task<string> checkVoteSuccess(string userId, Guid candidateId, Guid campaignId, Guid stageid)
        {
            string groupOfUser = "";
            var checkGroupUser = await dbContext.GroupUsers.Where(p => p.UserId == userId && p.CampaignId == campaignId).ToListAsync();
            foreach (var i in checkGroupUser)
            {
                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == i.GroupId && p.CampaignId == campaignId && p.IsVoter == true && p.IsStudentMajor == false).SingleOrDefaultAsync();
                if (checkGroup != null)
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

            if(groupOfCandidate.Equals("566fa89f-5730-45cc-b97d-2842ba1199e7") || groupOfCandidate.Equals("6101f9ff-55e1-4785-914f-216dadfbfae5")
                || groupOfCandidate.Equals("98d60b6d-5c5e-4cdb-b289-be92ffc77206") || groupOfCandidate.Equals("c5a820f6-1093-4355-80be-d814ae0dfad0")
                || groupOfCandidate.Equals("d8111aba-574e-4c2f-837a-e9a1cbfd36d2")) 
                groupCategoryOfCandidate = 1;

            var listVoteOfUser = await dbContext.Votings.Where(p => p.UserId == userId && p.StageId == stageid && p.Status == true).ToListAsync();

            int countdb = 0;
            int countcn = 0;
            foreach (var vote in listVoteOfUser)
            {
                var checkCandidateOfVote = await dbContext.Candidates.Where(p => p.CandidateId == vote.CandidateId && p.CampaignId == campaignId && p.Status == true).SingleOrDefaultAsync();
                var checkGroupCandidateOfVote = await dbContext.Groups.Where(p => p.GroupId == checkCandidateOfVote.GroupId && p.CampaignId == campaignId && p.IsVoter == false).SingleOrDefaultAsync();
                if (checkGroupCandidateOfVote.Name.Equals("566fa89f-5730-45cc-b97d-2842ba1199e7") || checkGroupCandidateOfVote.Name.Equals("6101f9ff-55e1-4785-914f-216dadfbfae5")
                    || checkGroupCandidateOfVote.Name.Equals("98d60b6d-5c5e-4cdb-b289-be92ffc77206") || checkGroupCandidateOfVote.Name.Equals("c5a820f6-1093-4355-80be-d814ae0dfad0")
                    || checkGroupCandidateOfVote.Name.Equals("d8111aba-574e-4c2f-837a-e9a1cbfd36d2"))
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
    }
}
