using Capstone_VotingSystem.Controllers;
using Capstone_VotingSystem.Models.RequestModels.QuestionRequest;
using Capstone_VotingSystem.Services.QuestionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/v1.0/questions")]
    [ApiController]
    public class QuestionController : BaseApiController
    {
        private readonly IQuestionService questionService;
        public QuestionController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }
        [HttpPost]
        [SwaggerOperation(summary: "Create new question and element")]
        public async Task<IActionResult> CreateQuestion(CreateQuestionRequest request)
        {
            try
            {
                var result = await questionService.CreateQuestion(request);
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
