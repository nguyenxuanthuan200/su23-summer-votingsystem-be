namespace Capstone_VotingSystem.Models.ResponseModels.UserResponse
{
    public class UpdateUserResponse
    {
        public string UserId { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public bool? Status { get; set; }
    }
}
