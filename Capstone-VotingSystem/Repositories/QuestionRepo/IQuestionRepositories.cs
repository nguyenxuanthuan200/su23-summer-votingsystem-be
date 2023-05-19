using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;
namespace Capstone_VotingSystem.Repositories.QuestionRepo
{
    public interface IQuestionRepositories
    {
        Task<IEnumerable<GetQuestionResponse>> GetQuestion();
    }
}
