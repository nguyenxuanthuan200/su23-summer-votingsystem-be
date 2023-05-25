namespace Capstone_VotingSystem.Models.ResponseModels.CampaignResponse
{
    public class GetCampaignResponse
    {
        public Guid Id { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public bool? Status { get; set; }
        public Guid? CampaignTypeId { get; set; }
        public Guid? CampusId { get; set; }
    }
}
