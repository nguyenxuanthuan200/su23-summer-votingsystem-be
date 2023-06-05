using AutoMapper;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.CandidateRepo
{
    public class CandidateService : ICandidateService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public CandidateService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
        }
        public async Task<CreateAccountCandidateResponse> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request)
        {
            var check = await dbContext.Accounts.Where(p => p.UserName == request.UserName).SingleOrDefaultAsync();
            if (check != null)
            {
                return null;
            }
            Account acc = new Account();
            {
                acc.UserName = request.UserName;
                acc.Password = request.Password;
                acc.Status = true;
            };
            var role= await dbContext.Roles.Where(p => p.Name.Equals("user")).SingleOrDefaultAsync();
            User us = new User();
            {
                us.UserName = request.UserName;
                us.Name = request.Name;
                us.Gender = request.Gender;
                us.Address = request.Address;
                us.CategoryId = request.CategoryId;
                us.RoleId = role.RoleId;

            }
            await dbContext.Accounts.AddAsync(acc);
            await dbContext.Users.AddAsync(us);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateAccountCandidateResponse>(us);
            return map;
        }

        public async Task<GetCandidateCampaignResponse> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            if (request.UserName == null || request.CampaignId == null)
            {
                return null;
            }
            var check = await dbContext.Users.Where(p => p.UserName == request.UserName).SingleOrDefaultAsync();
            if (check == null) return null;
            var id = Guid.NewGuid();
            CandidateProfile cam = new CandidateProfile();
            {
                cam.CandidateProfileId = id;
                cam.NickName = request.NickName;
                cam.Dob = DateTime.Now;
                cam.Image = request.Image;
                cam.UserName = request.UserName;
                cam.CampaignId = request.CampaignId;
            };
            Score score = new Score();
            {
                score.ScoreId = cam.CandidateProfileId;
            }
            {
                await dbContext.CandidateProfiles.AddAsync(cam);
                await dbContext.Scores.AddAsync(score);
                await dbContext.SaveChangesAsync();
                //var user = await dbContext.Users.Where(p => p.UserName == request.UserName).SingleOrDefaultAsync();
                var map = _mapper.Map<GetCandidateCampaignResponse>(cam);
                //map.Address = user.Address;
                return map;
            }
        }

        public Task<bool> DeleteCandidateCampaign(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetListCandidateCampaignResponse>> GetListCandidateCampaign(Guid campaignId)
        {
            var listCandidate = await dbContext.CandidateProfiles
               .Where(p => p.CampaignId == campaignId).ToListAsync();
            IEnumerable<GetListCandidateCampaignResponse> result = listCandidate.Select(
              x =>
              {
                  return new GetListCandidateCampaignResponse()
                  {
                      CandidateProfileId = x.CandidateProfileId,
                      NickName = x.NickName,
                      Dob = x.Dob,
                      Image = x.Image,
                      UserName = x.UserName,
              };
              }
              ).ToList();
            return result;
        }

        public async Task<UpdateCandidateProfileResponse> UpdateCandidateProfile(Guid id, UpdateCandidateProfileRequesst request)
        {
            var updateProfile = await dbContext.CandidateProfiles.Where(p => p.CandidateProfileId.Equals(id)).SingleOrDefaultAsync();
            if (updateProfile == null)
            {
                return null;
            }
            updateProfile.NickName = request.NickName;
            updateProfile.Dob = request.Dob;
            updateProfile.Image = request.Image;

            var user = await dbContext.Users.Where(p => p.UserName.Equals(updateProfile.UserName)).SingleOrDefaultAsync();
            user.Name = request.Name;
            user.Gender = request.Gender;
            user.Address = request.Address;
            dbContext.CandidateProfiles.Update(updateProfile);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var map = _mapper.Map<UpdateCandidateProfileResponse>(updateProfile);
            map.Name = user.Name;
            map.Gender = user.Gender;
            map.Address = user.Address;
          
            return map;
        }
    }
}
