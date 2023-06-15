using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Models.RequestModels.CategoryRequest;
using Capstone_VotingSystem.Models.ResponseModels.CategoryResponse;

namespace Capstone_VotingSystem.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<APIResponse<IEnumerable<GetCategoryResponse>>> GetCategory();
        Task<APIResponse<GetCategoryResponse>> CreateCategory(CreateCategoryRequest request);
        Task<APIResponse<GetCategoryResponse>> UpdateCategory(Guid id, UpdateCategoryRequest request);
    }
}
