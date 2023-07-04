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

        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? StatusUser { get; set; }
        public Guid? GroupId { get; set; }

    }
}
