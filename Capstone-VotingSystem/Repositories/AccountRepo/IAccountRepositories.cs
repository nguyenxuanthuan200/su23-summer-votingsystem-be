using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using Capstone_VotingSystem.Model.ResponseModel;

namespace Capstone_VotingSystem.Repositories.AccountRepo
{
    public interface IAccountRepositories
    {
        public Task<AccountResponse> CreateAccount(AccountModel model);


        public void DeleteAccount(Guid Id);
    }
}
