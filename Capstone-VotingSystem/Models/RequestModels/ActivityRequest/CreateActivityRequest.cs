namespace Capstone_VotingSystem.Models.RequestModels.ActivityRequest
{
    public class CreateActivityRequest
    {
        public Guid ActivityId { get; set; }
        public string Content { get; set; }
        public Guid CandidateId { get; set; }
    }
}
