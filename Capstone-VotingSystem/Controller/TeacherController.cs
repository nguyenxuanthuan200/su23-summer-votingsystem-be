using Capstone_VotingSystem.Repositories.TeacherRepo;
using Microsoft.AspNetCore.Mvc;


namespace Capstone_VotingSystem.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
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
                    //return StatusCode("Not Found", HttpStatusCode.NotFound);
                    return NotFound();
                //return StatusCode("Success", result, HttpStatusCode.OK);
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
