using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CandidateProfile;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistory;
using Capstone_VotingSystem.Models.ResponseModels.CandidateProfile;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace Capstone_VotingSystem.Repositories.CandidateProfileRepo
{
    public class CandidateProfileRepositories : ICandidateProfileRepositories
    {
        private readonly VotingSystemContext dbContext;

        public CandidateProfileRepositories( VotingSystemContext votingSystemContext)
        {
            this.dbContext = votingSystemContext;
        }
        public async Task<IEnumerable<CandidateProfileResponse>> GetAll()
        {
            var candidate = await dbContext.CandidateProfiles.ToListAsync();
            IEnumerable<CandidateProfileResponse> response = candidate.Select(x =>
            {
                return new CandidateProfileResponse()
                {
                    CandidateProfileId = x.CandidateProfileId,
                    NickName = x.NickName,
                    Dob = x.Dob,
                    Image = x.Image,
                    Username = x.Username,
                    CampaignId = x.CampaignId,

                };
            }).ToList();
            return response;
        }

        

        public async Task<IEnumerable<CandidateProfileResponse>> GetCandidateByCampaign(Guid campaignId)
        {
            var candidate = await dbContext.CandidateProfiles.Where(p => p.CampaignId.Equals(campaignId)).ToListAsync();
            IEnumerable<CandidateProfileResponse> response = candidate.Select(x =>
            {
                return new CandidateProfileResponse()
                {
                    CandidateProfileId = x.CandidateProfileId,
                    NickName = x.NickName,
                    Dob = x.Dob,
                    Image = x.Image,
                    Username = x.Username,
                    CampaignId = x.CampaignId,

                };
            }).ToList();
            return response;
        }

        public async Task<UpdateResponse> UpdateCandidate(Guid id, UpdateCandidateProfile update)
        {
            var updateProfile = await dbContext.CandidateProfiles.Where(p => p.CandidateProfileId.Equals(id)).SingleOrDefaultAsync();
            if (updateProfile == null)
            {
                return null;
            }
            updateProfile.NickName = update.NickName;
            updateProfile.Dob = update.Dob;
            updateProfile.Image = update.Image;
            dbContext.CandidateProfiles.Update(updateProfile);
            await dbContext.SaveChangesAsync();

            var candidateProfileId = Guid.NewGuid();
            UpdateResponse response = new UpdateResponse();
            {
                response.CandidateProfileId = candidateProfileId;
                response.NickName = update.NickName;
                response.Dob = update.Dob;
                response.Image = update.Image;
            }
         
           return response;

        }
    }
}
