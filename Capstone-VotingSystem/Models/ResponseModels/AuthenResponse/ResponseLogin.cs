namespace Capstone_VotingSystem.Models.ResponseModels.AuthenResponse
{
    public class ResponseLogin
    {
        public string? Token { get; set; }
        public ResponseAccountModel? Account { get; set; }
    }
}
