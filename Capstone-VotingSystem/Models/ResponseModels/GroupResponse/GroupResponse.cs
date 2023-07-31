namespace Capstone_VotingSystem.Models.ResponseModels.GroupResponse
{
    public class GroupResponse
    {
        public Guid GroupId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsVoter { get; set; }
    }
}
