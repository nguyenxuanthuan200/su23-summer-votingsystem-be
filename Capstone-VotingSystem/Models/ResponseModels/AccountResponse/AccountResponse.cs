namespace Capstone_VotingSystem.Models.ResponseModels.AccountResponse
{
    public class AccountResponse
    {
        public string UserName { get; set; } = null!;
        //public string? Password { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool? Status { get; set; }
        //public string? Token { get; set; }
        public Guid? RoleId { get; set; }
    }
}
