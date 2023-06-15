using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.CategoryRequest;
using Capstone_VotingSystem.Services.CategoryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }
        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        [SwaggerOperation(summary: "Get category")]
        public async Task<IActionResult> GetCategory()
        {
            try
            {
                var result = await categoryService.GetCategory();
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(summary: "Create Category")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            try
            {
                var response = await categoryService.CreateCategory(request);
                if (response.Success == false)
                {
                    return BadRequest(response);
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);

            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{categoryId}")]
        [SwaggerOperation(summary: "Update Category")]
        public async Task<IActionResult> UpdateCategory(Guid categoryId, UpdateCategoryRequest request)
        {
            try
            {
                var result = await categoryService.UpdateCategory(categoryId, request);
                if (result.Success == false)
                {
                    return BadRequest(result);
                }
                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }

    }
}
