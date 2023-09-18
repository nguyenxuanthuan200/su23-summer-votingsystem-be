﻿using AutoMapper;
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

            if (groupOfCandidate.Equals("Bộ môn Âm nhạc Truyền thống") || groupOfCandidate.Equals("Bộ môn Tiếng Anh dự bị") || groupOfCandidate.Equals("Bộ môn Kỹ năng mềm") || groupOfCandidate.Equals("Bộ môn Toán") || groupOfCandidate.Equals("Bộ môn Giáo dục thể chất"))
                groupCategoryOfCandidate = 1;

            var listVoteOfUser = await dbContext.Votings.Where(p => p.UserId == userId && p.StageId == stageid && p.Status == true).ToListAsync();

            int countdb = 0;
            int countcn = 0;
            foreach (var vote in listVoteOfUser)
            {
                var checkCandidateOfVote = await dbContext.Candidates.Where(p => p.CandidateId == vote.CandidateId && p.CampaignId == campaignId && p.Status == true).SingleOrDefaultAsync();
                var checkGroupCandidateOfVote = await dbContext.Groups.Where(p => p.GroupId == checkCandidateOfVote.GroupId && p.CampaignId == campaignId && p.IsVoter == false).SingleOrDefaultAsync();
                if (checkGroupCandidateOfVote.Name.Equals("Bộ môn Âm nhạc Truyền thống") || checkGroupCandidateOfVote.Name.Equals("Bộ môn Tiếng Anh dự bị") || checkGroupCandidateOfVote.Name.Equals("Bộ môn Kỹ năng mềm") || checkGroupCandidateOfVote.Name.Equals("Bộ môn Toán") || checkGroupCandidateOfVote.Name.Equals("Bộ môn Giáo dục thể chất"))
                    countdb = countdb + 1;
                else
                    countcn = countcn + 1;

            }

            if (groupOfUser.Equals("Kỳ dự bị") && groupCategoryOfCandidate == 1)
                return "success";
            if (groupOfUser.Equals("Kỳ chuyên ngành(HK1-HK6)") && groupCategoryOfCandidate == 1 && countdb == 0)
                return "success";
            if (groupOfUser.Equals("Kỳ chuyên ngành(HK1-HK6)") && groupCategoryOfCandidate == 0 && countcn <= 1)
                return "success";
            if (groupOfUser.Equals("Kỳ chuyên ngành(HK7-HK9)") && groupCategoryOfCandidate == 0 && countcn <= 2)
                return "success";

            return "false";

        }
    }
}
