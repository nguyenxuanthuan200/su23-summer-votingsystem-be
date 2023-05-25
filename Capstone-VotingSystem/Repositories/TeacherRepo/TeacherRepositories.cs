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
                        Id = x.TeacherId,
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
