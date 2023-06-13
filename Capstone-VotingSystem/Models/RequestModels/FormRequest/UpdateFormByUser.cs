namespace Capstone_VotingSystem.Models.RequestModels.FormRequest
{
    public class UpdateFormByUser
    {
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public bool? Visibility { get; set; }
    }
}
