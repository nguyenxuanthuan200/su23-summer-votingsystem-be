namespace Capstone_VotingSystem.Models.RequestModels.VoteRequest
{
    public class CreateVoteRequest
    {
        public DateTime? Time { get; set; }
        public Guid? TeacherCampaignId { get; set; }
        public string? MssvStudent { get; set; }
        public int? Answer { get; set; }
        public Guid? QuestionId { get; set; }
    }
}
