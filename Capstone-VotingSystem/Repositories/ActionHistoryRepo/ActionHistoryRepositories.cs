using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System.Drawing;
using System.Xml.Linq;

namespace Capstone_VotingSystem.Repositories.ActionHistoryRepo
{
    public class ActionHistoryRepositories : IActionHistoryRepositories
    {
        private readonly VotingSystemContext dbContext;

        public ActionHistoryRepositories(VotingSystemContext votingSystemContext) 
        {
            this.dbContext = votingSystemContext;
        }

        public async Task<IEnumerable<ActionHistoryResponse>> GetActionHistoryByUser(string username)
        {
            var actionHistory = await dbContext.ActionHistories.Where(p => p.Username.Equals(username)).ToListAsync();
            IEnumerable<ActionHistoryResponse> response = actionHistory.Select(x =>
            {
                return new ActionHistoryResponse()
                {
                    ActionHistoryId = x.ActionHistoryId,
                    Description = x.Description,
                    ActionTypeId = x.ActionTypeId,
                    Username = x.Username,
                };
            }).ToList();
            return response;
        }

        public async Task<IEnumerable<ActionHistoryResponse>> GetAllActionHistory()
        {
            var actionHistory = await dbContext.ActionHistories.ToListAsync();
            IEnumerable<ActionHistoryResponse> response = actionHistory.Select(x =>
            {
                return new ActionHistoryResponse()
                {
                    ActionHistoryId = x.ActionHistoryId,
                    Description = x.Description,
                    ActionTypeId = x.ActionTypeId,
                    Username = x.Username,
                };
            }).ToList();
            return response;
        }
    }
}
