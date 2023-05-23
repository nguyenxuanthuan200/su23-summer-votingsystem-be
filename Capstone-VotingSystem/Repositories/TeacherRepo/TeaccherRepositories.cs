using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Model.TeacherRespone;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.TeacherRepo
{
    public class TeaccherRepositories : ITeacherRepositories
    {
        private readonly VotingSystemContext dbContext;

        public TeaccherRepositories(VotingSystemContext dbContext)
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
                        Id = x.Id,
                        Name = x.Name,
                        AmountVote = x.AmountVote,
                        Score = x.Score,
                        DepartmentId = x.DepartmentId,
                        CampaignId = x.CampaignId,
                    };
                }
                ).ToList();
            return result;
        }
    }
}
