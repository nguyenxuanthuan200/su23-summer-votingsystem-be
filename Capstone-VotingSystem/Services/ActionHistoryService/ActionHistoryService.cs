using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;
using Microsoft.EntityFrameworkCore;


namespace Capstone_VotingSystem.Services.ActionHistoryService
{
    public class ActionHistoryService : IActionHistoryService
    {
        private readonly VotingSystemContext dbContext;

        public ActionHistoryService(VotingSystemContext votingSystemContext)
        {
            this.dbContext = votingSystemContext;
        }

        public async Task<IEnumerable<ActionHistoryResponse>> GetActionHistoryByUser(string username)
        {
            var actionHistory = await dbContext.HistoryActions.Where(p => p.UserId.Equals(username)).ToListAsync();
            IEnumerable<ActionHistoryResponse> response = actionHistory.Select(x =>
            {
                return new ActionHistoryResponse()
                {
                    HistoryActionId = x.HistoryActionId,
                    Description = x.Description,
                    TypeActionId = x.TypeActionId,
                    UserId = x.UserId,
                };
            }).ToList();
            return response;
        }

        public async Task<IEnumerable<ActionHistoryResponse>> GetAllActionHistory()
        {
            var actionHistory = await dbContext.HistoryActions.ToListAsync();
            IEnumerable<ActionHistoryResponse> response = actionHistory.Select(x =>
            {
                return new ActionHistoryResponse()
                {
                    HistoryActionId = x.HistoryActionId,
                    Description = x.Description,
                    TypeActionId = x.TypeActionId,
                    UserId = x.UserId,
                };
            }).ToList();
            return response;
        }
    }
}
