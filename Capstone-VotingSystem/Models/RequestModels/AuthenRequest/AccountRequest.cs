namespace Capstone_VotingSystem.Models.RequestModels.AuthenRequest
{
    public class AccountRequest
    {
        public string Username { get; set; } = null!;
        public string? Password { get; set; }
        public string? Token { get; set; }
        public bool? Status { get; set; }

        public Guid RoleId { get; set; } 
    }
}
