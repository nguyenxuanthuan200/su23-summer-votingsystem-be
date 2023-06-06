namespace Capstone_VotingSystem.Models.ResponseModels.AuthenResponse
{
    public class ResponseAccountModel
    {
        public string Token { get; set; } = string.Empty;
        public ResponseInfoUser? User { get; set; }
        public ResponseInfoRole? RoleUser { get; set; }
    }
}
