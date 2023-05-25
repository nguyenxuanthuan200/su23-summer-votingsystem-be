using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using Capstone_VotingSystem.Model.ResponseModel.AccountReponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.AccountRepo
{
    public class AccountRepositories : IAccountRepositories
    {
        private readonly VotingSystemContext _votingSystemContext;

        public AccountRepositories(VotingSystemContext votingSystemContext) 
        {
            _votingSystemContext = votingSystemContext;
        }
        public async Task<AccountResponse> CreateAccount(AccountModel model)
        {
          
            var result = await _votingSystemContext.AccountMods.SingleOrDefaultAsync(p => p.Username == model.Username);

            if (result != null)
            {
                return null;
            }
            var roleId = await _votingSystemContext.Roles.SingleOrDefaultAsync(p => p.Name.Equals(model.Role));
            var id =  Guid.NewGuid();
            if (roleId == null)
            {
                return null;
            }


            AccountMod user = new AccountMod();
            {
                user.RoleId = id;
                user.Username = model.Username;
                user.Password = model.Password;
                user.RoleId = roleId.RoleId;
                
            }
            await _votingSystemContext.AccountMods.AddAsync(user);
            await _votingSystemContext.SaveChangesAsync();
            AccountResponse response = new AccountResponse();
            {
                response.Username = model.Username;
                response.RoleName = roleId.Name;
            }
            return response;
        }

        public async void DeleteAccount(Guid Id)
        {
            var delete = _votingSystemContext.AccountMods
               .SingleOrDefault(p => p.RoleId == Id);
           // change status fals 
            //delete.Status = false;
            _votingSystemContext.AccountMods.Update(delete);
            await _votingSystemContext.SaveChangesAsync();

        }
    }
}
