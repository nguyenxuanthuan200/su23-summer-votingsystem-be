namespace Capstone_VotingSystem.Models.RequestModels.UserRequest
{
    public class UserPermissionRequest
    {
        public bool Voter { get; set; }
        public bool Candidate { get; set; }
        public bool Moderator { get; set; }
    }
}
