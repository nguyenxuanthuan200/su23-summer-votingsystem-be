using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;
using Capstone_VotingSystem.Models.ResponseModels.CandidateProfile;
using Capstone_VotingSystem.Models.ResponseModels.RateResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.RateCategoryRepo
{
    public class RatioRepositories : IRatioRepositories
    {
        private readonly VotingSystemContext dbContext;

        public RatioRepositories(VotingSystemContext votingSystemContext) 
        {
            this.dbContext = votingSystemContext;
        }
        public async Task<IEnumerable<RatioResponse>> GetAllRatio()
        {
            var actionHistory = await dbContext.RatioCategories.ToListAsync();
            IEnumerable<RatioResponse> response = actionHistory.Select(x =>
            {
                return new RatioResponse()
                {
                   RatioCategoryId = x.RatioCategoryId,
                   CampaignId = x.CampaignId,
                   RatioCategoryId1 = x.RatioCategoryId1,
                   RatioCategoryId2 = x.RatioCategoryId2,
                   Ratio = x.Ratio,
                   CheckRatio = x.CheckRatio,
                   Percent = x.Percent,
                };
            }).ToList();
            return response;
        }

        public async Task<IEnumerable<RatioResponse>> GetRatioById(Guid id)
        {
            var candidate = await dbContext.RatioCategories.Where(p => p.RatioCategoryId.Equals(id)).ToListAsync();
            IEnumerable<RatioResponse> response = candidate.Select(x =>
            {
                return new RatioResponse()
                {
                    RatioCategoryId = x.RatioCategoryId,
                    CampaignId = x.CampaignId,
                    RatioCategoryId1 = x.RatioCategoryId1,
                    RatioCategoryId2 = x.RatioCategoryId2,
                    Ratio = x.Ratio,
                    CheckRatio = x.CheckRatio,
                    Percent = x.Percent,

                };
            }).ToList();
            return response;
        }
    }
}
