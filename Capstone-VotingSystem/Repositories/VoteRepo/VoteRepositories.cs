using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Repositories.VoteRepo;

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
            var id = Guid.NewGuid();
            VoteDetail votedetail = new VoteDetail();
            {
                votedetail.VoteDetailId = id;
                votedetail.Time = DateTime.Now;
                votedetail.TeacherId = request.TeacherId;
                votedetail.Mssv = request.MssvStudent;
            };
            var idAnswerVote = Guid.NewGuid();
            AnswerVote answervote = new AnswerVote();
            {
                answervote.AnswerVoteId = idAnswerVote;
                answervote.Answer = request.Answer;
                answervote.QuestionId = request.QuestionId;
                answervote.VoteDetailId = id;
            }
            await dbContext.VoteDetails.AddAsync(votedetail);
            await dbContext.AnswerVotes.AddAsync(answervote);
            await dbContext.SaveChangesAsync();
            //var re = _mapper.map<createpostresponse>(post);
            //var mapproduct = _mapper.map<getproductresponse>(product);
            //re.product = mapproduct;

            return true;
        }

    }
}
