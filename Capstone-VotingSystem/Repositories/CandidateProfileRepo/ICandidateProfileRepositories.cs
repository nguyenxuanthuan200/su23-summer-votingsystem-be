﻿using Capstone_VotingSystem.Models.RequestModels.CandidateProfile;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;
using Capstone_VotingSystem.Models.ResponseModels.CandidateProfile;

namespace Capstone_VotingSystem.Repositories.CandidateProfileRepo
{
    public interface ICandidateProfileRepositories
    {
        public Task<IEnumerable<CandidateProfileResponse>> GetAll();

        public Task<IEnumerable<CandidateProfileResponse>> GetCandidateByCampaign(Guid campaignId);
        public Task<UpdateResponse> UpdateCandidate(Guid id, UpdateCandidateProfile update);
    }
}
