namespace Capstone_VotingSystem.Models.RequestModels.CampaignRequest
{
    public class CreateCampaignRequest
    {
        public string? Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Visibility { get; set; }
        //public bool? Status { get; set; }
        public string? UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
