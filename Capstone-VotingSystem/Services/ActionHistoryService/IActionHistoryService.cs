

using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;

namespace Capstone_VotingSystem.Services.ActionHistoryService
{
    public interface IActionHistoryService
    {
        public Task<IEnumerable<ActionHistoryResponse>> GetAllActionHistory();

        public Task<IEnumerable<ActionHistoryResponse>> GetActionHistoryByUser(string? username);
    }
}
