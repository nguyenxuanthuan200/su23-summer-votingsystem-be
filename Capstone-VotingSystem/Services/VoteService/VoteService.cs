using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.VoteService
{
    public class VoteService : IVoteService
    {
        private readonly VotingSystemContext dbContext;

        public VoteService(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> CreateVote(CreateVoteRequest request)
        {
            var user = await dbContext.Users.Where(
              p => p.UserId == request.UserName).SingleOrDefaultAsync();

            if (user == null)
            {
                return false;
            }
            var camp = await dbContext.Stages.Where(
              p => p.CampaignId == request.CampaignStageId).SingleOrDefaultAsync();

            if (camp == null)
            {
                return false;
            }
            var id = Guid.NewGuid();
            Voting vote = new Voting();
            {
                vote.VoringId = id;
                vote.SendingTime = request.Time;
                vote.StageId = request.CampaignStageId;
                vote.UserId = request.UserName;
            }
            await dbContext.Votings.AddAsync(vote);
            await dbContext.SaveChangesAsync();
            //VoteDetail votedetail = new VoteDetail()
            //{
            //    votedetail.VoteDetailId = id;
            //    votedetail.Time = DateTime.Now;
            //    votedetail.TeacherCampaignId = request.TeacherCampaignId;
            //    votedetail.Mssv = request.MssvStudent;
            //};
            //var idAnswerVote = Guid.NewGuid();
            //AnswerVote answervote = new AnswerVote();
            //{
            //    answervote.AnswerVoteId = idAnswerVote;
            //    answervote.Answer = request.Answer;
            //    answervote.QuestionStageId = request.QuestionId;
            //    answervote.VoteDetailId = id;
            //}
            //await dbContext.VoteDetails.AddAsync(votedetail);
            //await dbContext.AnswerVotes.AddAsync(answervote);
            //await dbContext.SaveChangesAsync();
            //var re = _mapper.map<createpostresponse>(post);
            //var mapproduct = _mapper.map<getproductresponse>(product);
            //re.product = mapproduct;

            return true;
        }

        public async Task<bool> CreateVoteDetail(CreateVoteDetailRequest request)
        {
            var checkvote = await dbContext.Votings.Where(
             p => p.VoringId == request.VotingId).SingleOrDefaultAsync();
            if (checkvote == null) return false;

            //var checkform = await dbContext.FormStages.Where(
            // p => p.FormStageId == request.FormStageId).SingleOrDefaultAsync();
            //if (checkform == null) return false;

            //var checkratio = await dbContext.RatioCategories.Where(
            // p => p.RatioCategoryId == request.RatioCategoryId).SingleOrDefaultAsync();
            //if (checkvote == null) return false;

            //var checkcandidate = await dbContext.CandidateProfiles.Where(
            // p => p.CandidateProfileId == request.CandidateProfileId).SingleOrDefaultAsync();
            //if (checkcandidate == null) return false;

            var id = Guid.NewGuid();
            VotingDetail votedetail = new VotingDetail();
            {
                votedetail.VotingDetailId = id;
                //votedetail.Time = request.Time;
                //votedetail.VotingId = request.VotingId;
                //votedetail.FormStageId = request.FormStageId;
                //votedetail.RatioCategoryId = request.RatioCategoryId;
                //votedetail.CandidateProfileId = request.CandidateProfileId;
            }
            await dbContext.VotingDetails.AddAsync(votedetail);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
