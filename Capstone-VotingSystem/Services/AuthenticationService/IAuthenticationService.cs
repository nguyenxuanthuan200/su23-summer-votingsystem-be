using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;

namespace Capstone_VotingSystem.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        public Task<Account> Login(LoginRequest model);
        public Task<ResponseAccount> GenerateToken(Account account);
        public Task<ResponseLogin> LoginFirebase(LoginFirebaseModel model);
    }
}
