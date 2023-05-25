using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AccountModRequest;
using Capstone_VotingSystem.Models.ResponseModels.AccountModResponse;

namespace Capstone_VotingSystem.Repositories.AccountModRepo
{
    public interface IAccountModRepositories
    {
        public Task<AccountModResponse> CreateAccount(CreateAccountModRequest createAccount);
    }
}
