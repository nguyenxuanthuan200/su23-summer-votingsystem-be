using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.ScoreRequest;
using Capstone_VotingSystem.Models.ResponseModels.ScoreResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.ScoreService
{
    public class ScoreService : IScoreService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public ScoreService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
        }
        public async Task<APIResponse<GetScoreResponse>> GetScore(GetScoreByCampaginRequest request)
        {
            APIResponse<GetScoreResponse> response = new();
            var checkCampagin = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId && p.Status == true && p.UserId == request.UserId).SingleOrDefaultAsync();
            if (checkCampagin == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc bạn không đủ quyền truy cập", StatusCodes.Status404NotFound);
                return response;
            }
            //var checkCampagin = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaginId && p.Status == true).SingleOrDefaultAsync();
            //if (checkCampagin == null)
            //{
            //    response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
            //    return response;
            //}
            GetScoreResponse scorerp = new();

            GetStageScoreResponse scoreInStage = new();
            List<GetStageScoreResponse> ListScoreInStage = new();

            ListCandidateScoreResponse candidateScore = new();
            List<ListCandidateScoreResponse> ListCandidateInStage = new();
            scorerp.CampaignId = checkCampagin.CampaignId;

            var StageInCampagin = await dbContext.Stages.Where(p => p.CampaignId == request.CampaignId && p.Status == true).ToListAsync();

            var CandidateInCampaign = await dbContext.Candidates.Where(p => p.CampaignId == request.CampaignId && p.Status == true).ToListAsync();

            foreach (var candidate in CandidateInCampaign)
            {
                int? score = 0;
                ListScoreInStage = new();
                foreach (var stage in StageInCampagin)
                {
                    var scoreStage = await dbContext.Scores.Where(p => p.StageId == stage.StageId && p.CandidateId == candidate.CandidateId).SingleOrDefaultAsync();
                    scoreInStage = new();
                    scoreInStage.StageId = stage.StageId;
                    scoreInStage.StageName = stage.Title;
                    if (scoreStage == null)
                    {
                        scoreInStage.StageScore = 0;
                    }
                    else
                        scoreInStage.StageScore = scoreStage.Score1;
                   // ListScoreInStage = new();
                    ListScoreInStage.Add(scoreInStage);
                    score += scoreInStage.StageScore;
                }
                var getName = await dbContext.Users.Where(p => p.UserId == candidate.UserId).SingleOrDefaultAsync();
                candidateScore = new();
                candidateScore.CandidateId = candidate.CandidateId;
                candidateScore.FullName = getName.FullName;
                candidateScore.TotalScore = score;
                candidateScore.listStageScore = ListScoreInStage;
                ListCandidateInStage.Add(candidateScore);


            }
            scorerp.listCandidateScore = ListCandidateInStage;
            //var sscoreInStage = await dbContext.Scores.Where(p => p.StageId == request.CampaginId && p.Status == true).SingleOrDefaultAsync();
            response.ToSuccessResponse("Lấy danh sách thành công", StatusCodes.Status200OK);
            response.Data = scorerp;
            return response;
        }

        public Task<APIResponse<GetScoreResponse>> SortScore(GetScoreByCampaginRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
