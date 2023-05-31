namespace Capstone_VotingSystem.Models.RequestModels.CampaignRequest
{
    public class GetCampaignByCampusRequest
    {
        public Guid CampaignId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? Endtime { get; set; }
        public bool? Status { get; set; }
        public Guid? CampusId { get; set; }
        public Guid? CampaignTypeId { get; set; }
    }
}
