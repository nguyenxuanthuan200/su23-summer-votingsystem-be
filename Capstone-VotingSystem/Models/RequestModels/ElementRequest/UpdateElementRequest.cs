namespace Capstone_VotingSystem.Models.RequestModels.ElementRequest
{
    public class UpdateElementRequest
    {
        public Guid ElementId { get; set; }
        public string? Answer { get; set; }
        public decimal? Rate { get; set; }
    }
}
