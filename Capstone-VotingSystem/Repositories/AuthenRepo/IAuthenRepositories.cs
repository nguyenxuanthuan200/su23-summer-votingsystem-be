using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public interface IAuthenRepositories
    {
        public Task<LoginResponse> GenerateTokenAsync(AccountMod accountMod);
        public Task<AccountMod> SignInAsync(LoginRequest payload);
    }
}
