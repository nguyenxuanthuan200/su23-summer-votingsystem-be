namespace Capstone_VotingSystem.Models.ResponseModels.AuthenResponse
{
    public class AccountResponse
    {
        public string Username { get; set; } = null!;
        public string? Token { get; set; }
        public bool? Status { get; set; }
        public string? roleName { get; set; }
        public ResponseInfoUser? user { get; set; }
    }
}
