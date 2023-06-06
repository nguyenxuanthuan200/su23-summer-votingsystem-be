namespace Capstone_VotingSystem.Models.ResponseModels.RateResponse
{
    public class RatioResponse
    {
        public Guid RatioCategoryId { get; set; }
        public decimal? Percent { get; set; }
        public double? Ratio { get; set; }
        public bool? CheckRatio { get; set; }
        public Guid? CategoryId1 { get; set; }
        public Guid? CategoryId2 { get; set; }
        public Guid? CampaignId { get; set; }
    }
}
