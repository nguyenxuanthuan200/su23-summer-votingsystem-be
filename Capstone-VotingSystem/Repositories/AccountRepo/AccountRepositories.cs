using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model;
using Capstone_VotingSystem.Model.ResponseModel;
using Microsoft.EntityFrameworkCore;
using Nest;

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
          
            var result = await _votingSystemContext.Accounts.SingleOrDefaultAsync(p => p.Username == model.Username);

            if (result != null)
            {
                return null;
            }
            var roleId = await _votingSystemContext.Roles.SingleOrDefaultAsync(p => p.Name.Equals("admin"));
            var id =  Guid.NewGuid();
            Account user = new Account();
            {
                user.Id = id;
                user.Username = model.Username;
                user.Password = model.Password;
                user.RoleId = roleId.Id;
                
            }
            await _votingSystemContext.Accounts.AddAsync(user);
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
            var delete = _votingSystemContext.Accounts
               .SingleOrDefault(p => p.Id == Id);
           // change status fals 
            //delete.Status = false;
            _votingSystemContext.Accounts.Update(delete);
            await _votingSystemContext.SaveChangesAsync();


        }
    }
}
