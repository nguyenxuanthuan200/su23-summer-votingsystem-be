namespace Capstone_VotingSystem.Models.ResponseModels.AuthenResponse
{
    public class ResponseInfoUser
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string Username { get; set; } = null!;
        public Guid? CategoryId { get; set; }
        public Guid? RoleId { get; set; }
    }
}
