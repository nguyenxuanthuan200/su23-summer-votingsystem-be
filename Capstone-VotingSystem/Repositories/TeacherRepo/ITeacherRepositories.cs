using Capstone_VotingSystem.Model.TeacherRespone;

namespace Capstone_VotingSystem.Repositories.TeacherRepo
{
    public interface ITeacherRepositories
    {
        Task<IEnumerable<GetListTeacherResponse>> GetListTeacher();

    }
}
