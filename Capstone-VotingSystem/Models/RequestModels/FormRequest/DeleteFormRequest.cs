namespace Capstone_VotingSystem.Models.RequestModels.FormRequest
{
    public class DeleteFormRequest
    {
        public Guid FormId { get; set; }
        public string UserId { get; set; }
    }
}
