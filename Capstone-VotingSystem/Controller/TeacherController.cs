using CoreApiResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Capstone_VotingSystem.Repositories.TeacherRepo;
using System.Net;
namespace Capstone_VotingSystem.Controller
{
    [Route("api/teacher")]
    [ApiController]
    public class TeacherController : BaseController
    {
        private readonly ITeacherRepositories teacherRepositories;
        public TeacherController(ITeacherRepositories teacherRepositories)
        {
            this.teacherRepositories = teacherRepositories;
        }
        [HttpGet]
        public async Task<IActionResult> GetListTeacher()
        {
            try
            {
                var result = await teacherRepositories.GetListTeacher();
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
