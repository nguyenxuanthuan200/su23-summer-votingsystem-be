using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.RateResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.RateCategoryRepo
{
    public class RatioCategoryService : IRatioCategoryService
    {
        private readonly VotingSystemContext dbContext;

        public RatioCategoryService(VotingSystemContext votingSystemContext)
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
                    CategoryId1 = x.CategoryId1,
                    CategoryId2 = x.CategoryId2,
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
                    CategoryId1 = x.CategoryId1,
                    CategoryId2 = x.CategoryId2,
                    Ratio = x.Ratio,
                    CheckRatio = x.CheckRatio,
                    Percent = x.Percent,

                };
            }).ToList();
            return response;
        }
    }
}
