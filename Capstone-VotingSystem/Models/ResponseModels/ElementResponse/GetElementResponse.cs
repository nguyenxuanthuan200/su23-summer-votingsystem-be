namespace Capstone_VotingSystem.Models.ResponseModels.ElementResponse
{
    public class GetElementResponse
    {
        public Guid ElementId { get; set; }
        public string? Content { get; set; }
        public bool? Status { get; set; }
        public Guid? QuestionId { get; set; }
        public decimal? Rate { get; set; }
    }
}
