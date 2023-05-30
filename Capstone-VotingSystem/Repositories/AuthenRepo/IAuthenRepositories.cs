using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using Octokit.Internal;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public interface IAuthenRepositories
    {
        public Task<LoginResponse> GenerateTokenAsync(AccountMod accountMod);
        Task<FirebaseResponse<LoginResponse>> Login(LoginFirebaseModel model);
        public Task<AccountMod> SignInAsync(LoginRequest payload);
    }
}
