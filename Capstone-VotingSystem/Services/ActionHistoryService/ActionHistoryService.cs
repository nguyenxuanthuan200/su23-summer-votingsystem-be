using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System.Drawing;
using System.Xml.Linq;

namespace Capstone_VotingSystem.Repositories.ActionHistoryRepo
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
            var actionHistory = await dbContext.ActionHistories.Where(p => p.UserName.Equals(username)).ToListAsync();
            IEnumerable<ActionHistoryResponse> response = actionHistory.Select(x =>
            {
                return new ActionHistoryResponse()
                {
                    ActionHistoryId = x.ActionHistoryId,
                    Description = x.Description,
                    ActionTypeId = x.ActionTypeId,
                    Username = x.UserName,
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
                    Username = x.UserName,
                };
            }).ToList();
            return response;
        }
    }
}
