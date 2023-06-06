using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteDetailRequest;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.RequestModels.VotingDetailRequest;
using Capstone_VotingSystem.Models.ResponseModels.VotingDetailResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.VoteRepo
{
    public class VotingDetailService : IVotingDetailService
    {
        private readonly VotingSystemContext dbContext;

        public VotingDetailService(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<VoteDetailResponse> CreateVotingDetail(CreateVoteDetailRequest request)
        {
            var check = await dbContext.Votings.Where(p => p.VotingId == request.VotingId).SingleOrDefaultAsync();
            if (check == null)
            {
                return null;
            }

            var id = Guid.NewGuid();
            VotingDetail votingDetail = new VotingDetail();
            {
                votingDetail.VotingDetailId = id;
                votingDetail.VotingId = request.VotingId;
                votingDetail.RatioCategoryId = request.RatioCategoryId;
                votingDetail.Time = request.Time;
                votingDetail.CandidateProfileId = request.CandidateProfileId;
                votingDetail.FormStageId = request.FormStageId;

            }


            VoteDetailResponse response = new VoteDetailResponse();
            {
                response.VotingDetailId = id;
                response.VotingId = request.VotingId;
                response.FormStageId = request.FormStageId;
                response.CandidateProfileId = request.CandidateProfileId;
                response.RatioCategoryId = request.RatioCategoryId;
                response.Time = request.Time;
            }
            await dbContext.VotingDetails.AddAsync(votingDetail);
            await dbContext.SaveChangesAsync();
            return response;
        }

        public async Task<IEnumerable<VoteDetailResponse>> GetAll()
        {
            var actionHistory = await dbContext.VotingDetails.ToListAsync();
            IEnumerable<VoteDetailResponse> response = actionHistory.Select(x =>
            {
                return new VoteDetailResponse()
                {
                    VotingDetailId = x.VotingDetailId,
                    CandidateProfileId = x.CandidateProfileId,
                    FormStageId = x.FormStageId,
                    VotingId = x.VotingId,
                    RatioCategoryId = x.RatioCategoryId,
                    Time = x.Time,

                };
            }).ToList();
            return response;
        }
    }
}
