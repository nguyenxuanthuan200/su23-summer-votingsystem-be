namespace Capstone_VotingSystem.Models.RequestModels.FormRequest
{
    public class CreateFormRequest
    {
        public string Name { get; set; }
        public string Visibility { get; set; }
        public Guid CategoryId { get; set; }
        public string UserId { get; set; }
    }
}
