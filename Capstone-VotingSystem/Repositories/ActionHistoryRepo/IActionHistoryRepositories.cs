using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;

namespace Capstone_VotingSystem.Repositories.ActionHistoryRepo
{
    public interface IActionHistoryRepositories
    {
        public Task<ActionHistoryResponse> GetActionHistorybyUsername();
    }
}
