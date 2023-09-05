namespace Capstone_VotingSystem.Models.ResponseModels.ActivityResponse
{
    public class GetActivityContentResponse
    {
        public Guid ActivityContentId { get; set; }
        public string Content { get; set; } = null!;
    }
}
