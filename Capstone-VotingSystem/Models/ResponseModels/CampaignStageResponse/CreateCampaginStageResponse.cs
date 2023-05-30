namespace Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse
{
    public class CreateCampaginStageResponse
    {
        public Guid CampaignStageId { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
