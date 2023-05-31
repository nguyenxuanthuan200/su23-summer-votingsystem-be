namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class GetCampaignResponse
    {
        public Guid CampaignId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool? Status { get; set; }
        public Guid? CampaignTypeId { get; set; }
        public Guid? CampusId { get; set; }
    }
}
