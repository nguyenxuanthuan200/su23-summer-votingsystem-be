using Capstone_VotingSystem.Entities;

namespace Capstone_VotingSystem.Model.ResponseModel.AuthenReponse
{
    public class LoginReponse
    {

        public Guid Id { get; set; }

        public string token { get; set; }

        public string roleName { get; set; }
    }
}
