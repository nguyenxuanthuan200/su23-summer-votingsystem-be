namespace Capstone_VotingSystem.Models.RequestModels.CandidateRequest
{
    public class UpdateCandidateProfileRequest
    {
        public string? Description { get; set; }
        public string? FullName { get; set; }
        public Guid GroupId { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
