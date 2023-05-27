using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.AccountModRequest;
using Capstone_VotingSystem.Models.ResponseModels.AccountModResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.AccountModRepo
{
    public class AccountModRepositories : IAccountModRepositories
    {
        private readonly VotingSystemContext DbContext;

        public AccountModRepositories(VotingSystemContext votingSystemContext) 
        {
            DbContext = votingSystemContext;
        }
        public async Task<AccountModResponse> CreateAccount(CreateAccountModRequest createAccount)
        {
            var result = await DbContext.AccountMods.SingleOrDefaultAsync(p => p.Username == createAccount.Username);

            if (result != null)
            {
                return null;
            }
            var roleId = await DbContext.Roles.SingleOrDefaultAsync(p => p.Name.Equals(createAccount.Role));
            var id = Guid.NewGuid();
            if (roleId == null)
            {
                return null;
            }


            AccountMod user = new AccountMod();
            {
                user.RoleId = id;
                user.Username = createAccount.Username;
                user.Password = createAccount.Password;
                user.RoleId = roleId.RoleId;

            }
            await DbContext.AccountMods.AddAsync(user);
            await DbContext.SaveChangesAsync();
            AccountModResponse response = new AccountModResponse();
            {
                response.Username = createAccount.Username;
                response.RoleName = roleId.Name;
            }
            return response;
        }
    }
}
