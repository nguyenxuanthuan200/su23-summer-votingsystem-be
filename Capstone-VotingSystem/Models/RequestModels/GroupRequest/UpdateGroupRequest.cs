namespace Capstone_VotingSystem.Models.RequestModels.GroupRequest
{
    public class UpdateGroupRequest
    {
        public string? Name { get; set; }
        public string? Description { get; set; }

        public bool? IsVoter { get; set; }
    }
}
