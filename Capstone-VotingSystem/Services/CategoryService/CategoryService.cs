using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CategoryRequest;
using Capstone_VotingSystem.Models.ResponseModels.CategoryResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;
        public CategoryService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<APIResponse<GetCategoryResponse>> CreateCategory(CreateCategoryRequest request)
        {
            APIResponse<GetCategoryResponse> response = new();
            var id = Guid.NewGuid();
            Category cate = new Category();
            {
                cate.CategoryId = id;
                cate.Description = request.Description;
                cate.Name = request.Name;

            }
            await dbContext.Categories.AddAsync(cate);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCategoryResponse>(cate);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetCategoryResponse>>> GetCategory()
        {
            APIResponse<IEnumerable<GetCategoryResponse>> response = new();
            var category = await dbContext.Categories.ToListAsync();
            if (category == null)
            {
                response.ToFailedResponse("Không có Category nào", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<GetCategoryResponse> result = category.Select(
                x =>
                {
                    return new GetCategoryResponse()
                    {
                        CategoryId = x.CategoryId,
                        Name = x.Name,
                        Description = x.Description,
                    };
                }
                ).ToList();
            response.Data = result;
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetCategoryResponse>> UpdateCategory(Guid id, UpdateCategoryRequest request)
        {
            APIResponse<GetCategoryResponse> response = new();
            var cam = await dbContext.Categories.SingleOrDefaultAsync(c => c.CategoryId == id);
            if (cam == null)
            {
                response.ToFailedResponse("Category không tồn tại hoặc đã bị xóa ", StatusCodes.Status400BadRequest);
                return response;
            }
            cam.Name = request.Name;
            cam.Description = request.Description;
            dbContext.Categories.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCategoryResponse>(cam);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
