using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Repositories.VoteRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

namespace Capstone_VotingSystem.Repositories.VoteRepo
{
    public class VoteRepositories : IVoteRepositories
    {
        private readonly VotingSystemContext dbContext;

        public VoteRepositories(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateVote(CreateVoteRequest request)
        {
            var check = await dbContext.Votings.Where(p => p.VotingId == request.VotingId).SingleOrDefaultAsync();
            if(check != null)
            {
                return false;
            }
           
            var id = Guid.NewGuid();
            VotingDetail votingDetail = new VotingDetail();
            {
                votingDetail.VotingDetailId = id;
                votingDetail.VotingId = request.VotingId;
                votingDetail.RatioCategoryId = request.RatioCategoryId;
                votingDetail.Time = request.Time;
                votingDetail.FormStageId = request.FormStageId;

            }
            var idAnswer = Guid.NewGuid();
            Answer answer = new Answer();
            {
                answer.AnswerId = idAnswer;
                answer.VotingDetailId = id;
                answer.AnswerSelect = request.AnswerSelect;
               
            }
            await dbContext.VotingDetails.AddAsync(votingDetail);
            await dbContext.Answers.AddAsync(answer);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
