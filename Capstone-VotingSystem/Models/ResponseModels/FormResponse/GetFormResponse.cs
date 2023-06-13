namespace Capstone_VotingSystem.Models.ResponseModels.FormResponse
{
    public class GetFormResponse
    {
        public Guid FormId { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public bool? Visibility { get; set; }
    }
}
