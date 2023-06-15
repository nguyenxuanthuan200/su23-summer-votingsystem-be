namespace Capstone_VotingSystem.Models.ResponseModels.CategoryResponse
{
    public class GetCategoryResponse
    {
        public Guid CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
