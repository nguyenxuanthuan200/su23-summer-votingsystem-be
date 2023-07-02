namespace Capstone_VotingSystem.Models.RequestModels.ActionHistoryRequest
{
    public class UpdateActionHistoryRequest
    {
        public string? Description { get; set; }
        public Guid? TypeActionId { get; set; }
    }
}
