using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model.ResponseModel.AuthenReponse;
using Capstone_VotingSystem.Model.ResquestModel.AuthenRequest;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public interface IAuthenticationRepositories
    {
        public Task<LoginReponse> GenerateTokenAsync(Account account);
        public Task<AccountMod> SignInAsync(LoginRequest payload);

        
    }
}
