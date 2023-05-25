namespace Capstone_VotingSystem.Models.ResponseModels.AuthenResponse
{
    public class LoginResponse
    {
        public Guid Id { get; set; }

        public string token { get; set; }

        public string roleName { get; set; }
    }
}
