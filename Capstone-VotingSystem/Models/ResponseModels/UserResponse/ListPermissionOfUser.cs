namespace Capstone_VotingSystem.Models.ResponseModels.UserResponse
{
    public class ListPermissionOfUser
    {
        public bool Voter { get; set; }
        public bool Candidate { get; set; }
        public bool Moderator { get; set; }
    }
}
