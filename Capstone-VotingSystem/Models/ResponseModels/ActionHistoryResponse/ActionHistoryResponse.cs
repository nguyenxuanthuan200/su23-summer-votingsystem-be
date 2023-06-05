namespace Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse
{
    public class ActionHistoryResponse
    {
        public Guid ActionHistoryId { get; set; }
        public string? Description { get; set; }
        public Guid? ActionTypeId { get; set; }
        public string? Username { get; set; }
    }
}
