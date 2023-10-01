namespace Capstone_VotingSystem.Models.RequestModels.AccountRequest
{
    public class CreateAccountRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public string RoleName { get; set; }
    }
}
