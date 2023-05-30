namespace Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse
{
    public class GetCampaignStageByCampaignResponse
    {
        public Guid CampaignStageId { get; set; }
        public Guid? CampaignId { get; set; }
        public double? AmountVote { get; set; }
    }
}
