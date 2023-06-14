namespace Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse
{
    public class ActionHistoryResponse
    {
        public Guid HistoryActionId { get; set; }
        public string? Description { get; set; }
        public Guid? TypeActionId { get; set; }
        public string? UserId { get; set; }
    }
}
