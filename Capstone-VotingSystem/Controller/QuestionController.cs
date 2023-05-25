using Capstone_VotingSystem.Repositories.QuestionRepo;
using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Capstone_VotingSystem.Controller
{
    [Route("api/question")]
    [ApiController]
    public class QuestionController : BaseController
    {
        private readonly IQuestionRepositories questionRepositories;
        public QuestionController(IQuestionRepositories questionRepositories)
        {
            this.questionRepositories = questionRepositories;
        }
        [HttpGet]
        public async Task<IActionResult> GetQuestion()
        {
            try
            {
                var result = await questionRepositories.GetQuestion();
                if (result == null)
                    return CustomResult("Not Found", HttpStatusCode.NotFound);
                return CustomResult("Success", result, HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database.");
            }
        }
    }
}
