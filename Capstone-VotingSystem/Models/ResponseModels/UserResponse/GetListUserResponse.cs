namespace Capstone_VotingSystem.Models.ResponseModels.UserResponse
{
    public class GetListUserResponse
    {
        public string UserName { get; set; }
        public DateTime? CreateAt { get; set; }
        public Guid? RoleId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public bool Status { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public ListPermissionOfUser Permission { get; set; }
    }
}
