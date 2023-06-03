using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.ActionHistoryRepo
{
    public class ActionHistoryRepositories : IActionHistoryRepositories
    {
        private readonly VotingSystemContext dbContext;

        public ActionHistoryRepositories(VotingSystemContext votingSystemContext) 
        {
            this.dbContext = votingSystemContext;
        }

        public Task<ActionHistoryResponse> GetActionHistorybyUsername()
        {
            throw new NotImplementedException();
        }
    }
}
