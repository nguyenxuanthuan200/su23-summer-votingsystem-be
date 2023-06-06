using Capstone_VotingSystem.Models.ResponseModels.RateResponse;

namespace Capstone_VotingSystem.Repositories.RateCategoryRepo
{
    public interface IRatioCategoryService
    {
        public Task<IEnumerable<RatioResponse>> GetAllRatio();

        public Task<IEnumerable<RatioResponse>> GetRatioById(Guid id);
    }
}
