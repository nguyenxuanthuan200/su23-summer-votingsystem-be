using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;
using Capstone_VotingSystem.Models.ResponseModels.RateResponse;

namespace Capstone_VotingSystem.Repositories.RateCategoryRepo
{
    public interface IRatioRepositories
    {
        public Task<IEnumerable<RatioResponse>> GetAllRatio();

        public Task<IEnumerable<RatioResponse>> GetRatioById(Guid id);
    }
}
