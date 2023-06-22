namespace Capstone_VotingSystem.Models.RequestModels.FormRequest
{
    public class UpdateFormByUser
    {
        public string? Name { get; set; }
        public string? UserId { get; set; }
        public string? Visibility { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
