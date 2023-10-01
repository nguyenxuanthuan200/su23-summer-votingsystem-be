namespace Capstone_VotingSystem.Models.ResponseModels.ActivityResponse
{
    public class GetActivityResponse
    {
        public Guid ActivityId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
