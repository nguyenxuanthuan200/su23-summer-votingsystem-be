using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.StatisticalResponse;
using Capstone_VotingSystem.Models.ResponseModels.VotingResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.StatisticalService
{
    public class StatisticalService : IStatisticalService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public StatisticalService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
        }
        public async Task<APIResponse<IEnumerable<GetResultCampaignResponse>>> GetResultCampaign(Guid campaignId)
        {
            APIResponse<IEnumerable<GetResultCampaignResponse>> response = new();
            var checkcam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.CampaignId == campaignId);
            if (checkcam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkcam.PublishTheResult == false)
            {
                response.ToFailedResponse("Kết quả của chiến dịch được ẩn. Bạn không thể xem chúng!!", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkcam.Process != "Đã kết thúc")
            {
                response.ToFailedResponse("Chiến dịch chưa kết thúc. Bạn không thể xem chúng!!", StatusCodes.Status400BadRequest);
                return response;
            }

            var listCandidate = dbContext.Scores.GroupBy(score => score.CandidateId).Select(group => new
            {
                CandidateId = group.Key,
                TotalPoints = group.Sum(score => score.Point)
            })
    .OrderByDescending(group => group.TotalPoints)
    .Take(10)
    .Join(dbContext.Candidates.Where(p => p.CampaignId == campaignId), group => group.CandidateId, candidate => candidate.CandidateId, (group, candidate) => candidate)
    .ToList();
            List<GetResultCampaignResponse> result = new List<GetResultCampaignResponse>();
            foreach (var item in listCandidate)
            {


                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == item.GroupId).SingleOrDefaultAsync();
                var candidate = new GetResultCampaignResponse();
                {
                    candidate.Description = item.Description;
                    candidate.GroupName = checkGroup != null ? checkGroup.Name : null;
                    candidate.FullName = item.FullName;
                    candidate.AvatarUrl = item.AvatarUrl;
                }
                result.Add(candidate);

                var scoreStage = await dbContext.Scores.Where(p => p.CandidateId == item.CandidateId).ToListAsync();
                int score = 0;
                foreach (var a in scoreStage)
                {
                    score = (int)a.Point;
                }
                candidate.Score = score;
            }
            if (result.Count() == 0)
            {
                response.ToFailedResponse("Không có ứng cử viên nào trong chiến dịch", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<StatisticalTotalResponse>> StatisticalTotal()
        {
            APIResponse<StatisticalTotalResponse> response = new();
            StatisticalTotalResponse result = new();
            var checkcam = dbContext.Campaigns.Where(p => p.Status == true).Count();
            result.Campaign = checkcam != null ? checkcam : 0;
            var checkform = dbContext.Forms.Where(p => p.Status == true).Count();
            result.Form = checkform != null ? checkform : 0;
            var checkcandidate = dbContext.Candidates.Where(p => p.Status == true).Count();
            result.Candidate = checkcandidate != null ? checkcandidate : 0;
            var checkvoter = dbContext.Users.Where(p => p.Status == true).Count();
            result.Voter = checkvoter != null ? checkvoter : 0;

            response.ToSuccessResponse(response.Data = result, "Thống kê thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<TotalVoterInCampaignResponse>>> StatisticalVoteOfCandidateGroup(Guid campaignId)
        {
            APIResponse<IEnumerable<TotalVoterInCampaignResponse>> response = new();
            List<TotalVoterInCampaignResponse> result = new();
            var checkcam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.CampaignId == campaignId);
            if (checkcam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var GetListStage = await dbContext.Stages.Where(p => p.CampaignId == campaignId && p.Status == true).ToListAsync();
            if (GetListStage.Count <= 0)
            {
                response.ToFailedResponse("Không thể thống kê chiến dịch không có giai đoạn nào", StatusCodes.Status400BadRequest);
                return response;
            }
            var ListSaInGroup = new List<TotalVoterInCampaignResponse>();
            var listGroupVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == false).ToListAsync();
            foreach (var group in listGroupVoterOfCampagin)
            {
                var SaInGroup = new TotalVoterInCampaignResponse();
                SaInGroup.GroupId = group.GroupId;
                SaInGroup.GroupName = group.Name;
                SaInGroup.Total = 0;
                ListSaInGroup.Add(SaInGroup);

            }
            int cout = 0;
            foreach (var item in GetListStage)
            {
                var CountVoteByStage = await dbContext.Votings.Where(p => p.StageId == item.StageId && p.Status == true).ToListAsync();
                //.DistinctBy(x => x.UserId)
                foreach (var i in CountVoteByStage)
                {
                    var checkCandidate = await dbContext.Candidates.Where(p => p.CandidateId == i.CandidateId && p.Status == true).SingleOrDefaultAsync();
                    if (checkCandidate == null)
                    {
                        response.ToSuccessResponse(response.Data = ListSaInGroup, "Lấy danh sách thành công", StatusCodes.Status200OK);
                        return response;
                    }
                    var checkGroup = await dbContext.Groups.Where(p => p.GroupId == checkCandidate.GroupId).SingleOrDefaultAsync();
                    if (checkGroup == null)
                    {
                        response.ToSuccessResponse(response.Data = ListSaInGroup, "Lấy danh sách thành công", StatusCodes.Status200OK);
                        return response;
                    }
                    var index = ListSaInGroup.FindIndex(p => p is TotalVoterInCampaignResponse && ((TotalVoterInCampaignResponse)p).GroupId == checkGroup.GroupId);
                        if (index != -1)
                        {
                            ((TotalVoterInCampaignResponse)ListSaInGroup[index]).Total += 1;
                        cout = cout + 1;
                        }

                }

            }
            response.ToSuccessResponse(response.Data = ListSaInGroup, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<StatisticalVoterJoinCampaignResponse>> StatisticalVoterJoinCampaign(Guid campaignId)
        {
            APIResponse<StatisticalVoterJoinCampaignResponse> response = new();
            StatisticalVoterJoinCampaignResponse result = new();
            var checkcam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.CampaignId == campaignId);
            if (checkcam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var ListSaInGroup = new List<TotalVoterInCampaignResponse>();
            var listGroupVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == true && p.IsStudentMajor == false).ToListAsync();
            foreach (var group in listGroupVoterOfCampagin)
            {
                var SaInGroup = new TotalVoterInCampaignResponse();
                SaInGroup.GroupId = group.GroupId;
                SaInGroup.GroupName = group.Name;
                SaInGroup.Total = 0;
                ListSaInGroup.Add(SaInGroup);

            }
            var ListGroupMajor = new List<TotalVoterInCampaignResponse>();
            var listGroupMajorVoterOfCampagin = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == true && p.IsStudentMajor == true).ToListAsync();
            foreach (var group in listGroupMajorVoterOfCampagin)
            {
                var GroupMajor = new TotalVoterInCampaignResponse();
                GroupMajor.GroupId = group.GroupId;
                GroupMajor.GroupName = group.Name;
                GroupMajor.Total = 0;
                ListGroupMajor.Add(GroupMajor);

            }

            var CountVoteInGroup = await dbContext.GroupUsers.Where(p => p.CampaignId == campaignId).ToListAsync();
            foreach (var e in CountVoteInGroup)
            {
                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == e.GroupId && p.IsVoter == true).SingleOrDefaultAsync();
                if (checkGroup != null && checkGroup.IsStudentMajor == false)
                {
                    var index = ListSaInGroup.FindIndex(p => p is TotalVoterInCampaignResponse && ((TotalVoterInCampaignResponse)p).GroupId == checkGroup.GroupId);
                    if (index != -1)
                    {
                        ((TotalVoterInCampaignResponse)ListSaInGroup[index]).Total += 1;
                    }
                }
                if (checkGroup != null && checkGroup.IsStudentMajor == true)
                {
                    var index = ListGroupMajor.FindIndex(p => p is TotalVoterInCampaignResponse && ((TotalVoterInCampaignResponse)p).GroupId == checkGroup.GroupId);
                    if (index != -1)
                    {
                        ((TotalVoterInCampaignResponse)ListGroupMajor[index]).Total += 1;
                    }
                }
            }
            result.GroupOfVoter = ListSaInGroup;
            result.GroupMajorOfVoter = ListGroupMajor;

            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;

        }

    }
}
