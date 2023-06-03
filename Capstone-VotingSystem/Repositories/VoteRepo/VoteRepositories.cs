﻿using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.VoteRequest;
using Capstone_VotingSystem.Models.ResponseModels.VoteResponse;
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

        public async Task<VoteDetailResponse> CreateVote(CreateVoteRequest request)
        {
            var check = await dbContext.Votings.Where(p => p.VotingId == request.VotingId).SingleOrDefaultAsync();
            if(check == null)
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
                votingDetail.FormStageId = request.FormStageId;

            }
           
            
            VoteDetailResponse response = new VoteDetailResponse();
            {
                response.VotingDetailId = id;
                response.VotingId = request.VotingId;
                response.FormStageId =request.FormStageId;
                response.CandidateProfileId = request.CandidateProfileId;
                response.RatioCategoryId = request.RatioCategoryId;
                response.Time = request.Time;
            }
            await dbContext.VotingDetails.AddAsync(votingDetail);
            await dbContext.SaveChangesAsync();
            return response;
        }
    }
}
