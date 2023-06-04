using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AuthenRequest;
using Capstone_VotingSystem.Models.ResponseModels.AuthenResponse;
using NuGet.Common;
using Octokit.Internal;
using System.Threading.Tasks;

namespace Capstone_VotingSystem.Repositories.AuthenRepo
{
    public interface IAuthenticationService
    {
       public Task<Account> Login(LoginRequest model);
       public Task<ResponseAccount> GenerateToken(Account account);
       public Task<ResponseLogin> LoginFirebase(LoginFirebaseModel model);
    }
}
