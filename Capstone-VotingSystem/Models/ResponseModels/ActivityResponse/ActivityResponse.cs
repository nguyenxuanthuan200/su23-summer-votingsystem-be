namespace Capstone_VotingSystem.Models.ResponseModels.ActivityResponse
{
    public class ActivityResponse
    {
        public Guid ActivityId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public Guid? CandidateId { get; set; }
    }
}
