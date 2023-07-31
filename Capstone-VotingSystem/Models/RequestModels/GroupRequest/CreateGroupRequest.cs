namespace Capstone_VotingSystem.Models.RequestModels.GroupRequest
{
    public class CreateGroupRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool IsVoter { get; set; }
        public Guid CampaignId { get; set; }
    }
}
