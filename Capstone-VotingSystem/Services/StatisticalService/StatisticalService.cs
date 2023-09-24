using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.StatisticalResponse;
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
            if (result.Count()==0)
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

            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
