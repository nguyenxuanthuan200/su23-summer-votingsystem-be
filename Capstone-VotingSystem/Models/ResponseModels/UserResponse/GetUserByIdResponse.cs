namespace Capstone_VotingSystem.Models.ResponseModels.UserResponse
{
    public class GetUserByIdResponse
    {
        public string UserId { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
