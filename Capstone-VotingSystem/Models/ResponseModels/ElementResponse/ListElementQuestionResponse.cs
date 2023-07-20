namespace Capstone_VotingSystem.Models.ResponseModels.ElementResponse
{
    public class ListElementQuestionResponse
    {
        public Guid ElementId { get; set; }
        public string? Answer { get; set; }
        public bool? Status { get; set; }
        public decimal? Rate { get; set; }
    }
}
