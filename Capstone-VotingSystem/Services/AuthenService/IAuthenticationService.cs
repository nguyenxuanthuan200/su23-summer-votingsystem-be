using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using Octokit.Internal;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public interface IAuthenticationService
    {
        public Task<Account> Login(LoginRequest model);
        public Task<AccountResponse> GenerateToken(Account account);
        public Task<LoginResponse> LoginFirebase(LoginFirebaseModel model);
    }
}
