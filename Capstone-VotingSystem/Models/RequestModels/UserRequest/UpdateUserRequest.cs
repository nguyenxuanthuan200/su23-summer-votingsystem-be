namespace Capstone_VotingSystem.Models.RequestModels.UserRequest
{
    public class UpdateUserRequest
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public DateTime? Dob { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
