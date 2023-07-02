namespace Capstone_VotingSystem.Models.RequestModels.ActionHistoryRequest
{
    public class ActionHistoryRequest
    {
        public string? Description { get; set; }
        //public DateTime? Time { get; set; }
        public Guid? TypeActionId { get; set; }
        public string? UserId { get; set; }
    }
}
