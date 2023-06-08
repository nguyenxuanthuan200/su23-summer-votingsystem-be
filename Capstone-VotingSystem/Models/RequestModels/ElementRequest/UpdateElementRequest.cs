namespace Capstone_VotingSystem.Models.RequestModels.ElementRequest
{
    public class UpdateElementRequest
    {
        public Guid ElementId { get; set; }
        public string? Text { get; set; }
    }
}
