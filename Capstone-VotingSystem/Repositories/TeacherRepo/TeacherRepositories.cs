using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.TeacherResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.TeacherRepo
{
    public class TeacherRepositories : ITeacherRepositories

    {
        private readonly VotingSystemContext dbContext;

        public TeacherRepositories(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<GetListTeacherResponse>> GetListTeacher()
        {
            var teacher = await dbContext.Teachers.ToListAsync();
            IEnumerable<GetListTeacherResponse> result = teacher.Select(
                x =>
                {
                    return new GetListTeacherResponse()
                    {
                        TeacherId = x.TeacherId,
                        Name = x.Name,                       
                        Email = x.Email,
                        Img = x.Img,
                        CampusDepartmentId = x.CampusDepartmentId,
                    };
                }
                ).ToList();
            return result;
        }
    }
}
