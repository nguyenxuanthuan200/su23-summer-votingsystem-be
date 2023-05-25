namespace Capstone_VotingSystem.Models.RequestModels.AccountModRequest
{
    public class CreateAccountModRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string Role { get; set; }
    }
}
