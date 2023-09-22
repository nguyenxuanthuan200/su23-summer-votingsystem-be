using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.AccountRequest;
using Capstone_VotingSystem.Models.ResponseModels.AccountResponse;

namespace Capstone_VotingSystem.Services.AccountService
{
    public interface IAccountService
    {
        //public Task<APIResponse<IEnumerable<AccountResponse>>> GetAllAcount();
        public Task<APIResponse<string>> CreateAccount(CreateAccountRequest request);

        public Task<APIResponse<string>> BanAccount(string id);
        public Task<APIResponse<string>> UnbanAccount(string id);
    }
}
