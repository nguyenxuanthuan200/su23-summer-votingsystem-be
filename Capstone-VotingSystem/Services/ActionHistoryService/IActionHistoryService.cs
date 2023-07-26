

using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;

namespace Capstone_VotingSystem.Services.ActionHistoryService
{
    public interface IActionHistoryService
    {
        public Task<APIResponse<IEnumerable<ActionHistoryResponse>>> GetActionHistoryByUser(string? userId);
    }
}
