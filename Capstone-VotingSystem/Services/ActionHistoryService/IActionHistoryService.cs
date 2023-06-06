

using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;

namespace Capstone_VotingSystem.Repositories.ActionHistoryRepo
{
    public interface IActionHistoryService
    {
        public Task<IEnumerable<ActionHistoryResponse>> GetAllActionHistory();

        public Task<IEnumerable<ActionHistoryResponse>> GetActionHistoryByUser(string? username);
    }
}
