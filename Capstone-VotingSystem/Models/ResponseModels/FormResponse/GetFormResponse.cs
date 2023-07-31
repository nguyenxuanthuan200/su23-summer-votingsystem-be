namespace Capstone_VotingSystem.Models.ResponseModels.FormResponse
{
    public class GetFormResponse
    {
        public Guid FormId { get; set; }
        public string? Name { get; set; }
        public Guid? CategoryId { get; set; }
        public bool IsApprove { get; set; }
        public string? UserId { get; set; }
        public string? Visibility { get; set; }
    }
}
