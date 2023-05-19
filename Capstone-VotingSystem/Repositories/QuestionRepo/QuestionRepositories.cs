using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.QuestionRepo
{
    public class QuestionRepositories : IQuestionRepositories
    {
        private readonly VotingSystemContext dbContext;

        public QuestionRepositories(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<GetQuestionResponse>> GetQuestion()
        {
            var question = await dbContext.Questions.ToListAsync();
            IEnumerable<GetQuestionResponse> result = question.Select(
                x =>
                {
                    return new GetQuestionResponse()
                    {
                        Id = x.Id,
                        Question1 = x.Question1,
                        Description = x.Description,
                        CampaignId = x.CampaignId,
                    };
                }
                ).ToList();
            return result;
        }
    }
}
