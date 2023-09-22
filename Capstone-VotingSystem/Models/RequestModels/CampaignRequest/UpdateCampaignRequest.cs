namespace Capstone_VotingSystem.Models.RequestModels.CampaignRequest
{
    public class UpdateCampaignRequest
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Visibility { get; set; }
        public Guid CategoryId { get; set; }
        public bool VisibilityCandidate { get; set; }
        public bool PublishTheResult { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
