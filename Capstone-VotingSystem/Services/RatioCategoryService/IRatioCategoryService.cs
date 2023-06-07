using Capstone_VotingSystem.Models.ResponseModels.RateResponse;

namespace Capstone_VotingSystem.Services.RateCategoryService
{
    public interface IRatioCategoryService
    {
        public Task<IEnumerable<RatioResponse>> GetAllRatio();

        public Task<IEnumerable<RatioResponse>> GetRatioById(Guid id);
    }
}
