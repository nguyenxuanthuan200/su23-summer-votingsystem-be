using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;

namespace Capstone_VotingSystem.Services.AccountService
{
    public interface IAccountService
    {
        public Task<APIResponse<IEnumerable<AccountResponse>>> GetAllAcount();

        public Task<APIResponse<string>> BanAccount(string id);
    }
}
