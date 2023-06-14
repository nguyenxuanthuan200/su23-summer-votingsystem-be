using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.CandidateService
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
        public async Task<APIResponse<CreateAccountCandidateResponse>> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request)
        {
            APIResponse<CreateAccountCandidateResponse> response = new();
            var check = await dbContext.Accounts.Where(p => p.UserName == request.UserName).SingleOrDefaultAsync();
            if (check != null)
            {
                response.ToFailedResponse("UserName đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            Account acc = new Account();
            {
                acc.UserName = request.UserName;
                acc.Password = request.Password;
                acc.Status = true;
            };
            var checkcategory = await dbContext.Categories.Where(p => p.CategoryId == request.CategoryId).SingleOrDefaultAsync();
            if (checkcategory == null)
            {
                response.ToFailedResponse("Category không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var role = await dbContext.Roles.Where(p => p.Name.Equals("user")).SingleOrDefaultAsync();
            User us = new User();
            {
                //us.UserName = request.UserName;
                //us.Name = request.Name;
                //us.Gender = request.Gender;
                //us.Address = request.Address;
                //us.CategoryId = request.CategoryId;
                //us.RoleId = role.RoleId;

            }
            await dbContext.Users.AddAsync(us);
            await dbContext.Accounts.AddAsync(acc);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateAccountCandidateResponse>(us);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;

        }

        public async Task<APIResponse<GetCandidateCampaignResponse>> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            //APIResponse<GetCandidateCampaignResponse> response = new();
            //if (request.UserName == null || request.CampaignId == null)
            //{
            //    response.ToFailedResponse("UserName hoặc Campaign Không được bỏ trống", StatusCodes.Status400BadRequest);
            //    return response;
            //}
            //var check = await dbContext.Users.Where(p => p.UserName == request.UserName).SingleOrDefaultAsync();
            //if (check == null)
            //{
            //    response.ToFailedResponse("UserName không đúng!!", StatusCodes.Status404NotFound);
            //    return response;
            //}
            //var check2 = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId).SingleOrDefaultAsync();
            //if (check2 == null)
            //{
            //    response.ToFailedResponse("Campaign không đúng!!", StatusCodes.Status404NotFound);
            //    return response;
            //}
            //var id = Guid.NewGuid();
            //CandidateProfile cam = new CandidateProfile();
            //{
            //    cam.CandidateProfileId = id;
            //    cam.NickName = request.NickName;
            //    cam.Dob = DateTime.Now;
            //    cam.Image = request.Image;
            //    cam.UserName = request.UserName;
            //    cam.CampaignId = request.CampaignId;
            //};
            //Score score = new Score();
            //{
            //    score.ScoreId = cam.CandidateProfileId;
            //}
            //{
            //    await dbContext.CandidateProfiles.AddAsync(cam);
            //    await dbContext.Scores.AddAsync(score);
            //    await dbContext.SaveChangesAsync();
            //    var map = _mapper.Map<GetCandidateCampaignResponse>(cam);
            //    response.ToSuccessResponse("Thành công!!", StatusCodes.Status200OK);
            //    response.Data = map;
            //    return response;
            //}
            return null;
        }

        public Task<bool> DeleteCandidateCampaign(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetListCandidateCampaign(Guid campaignId)
        {
            //APIResponse<IEnumerable<GetListCandidateCampaignResponse>> response = new();
            //var checkcam =await dbContext.Campaigns.SingleOrDefaultAsync(p => p.CampaignId == campaignId);
            //if (checkcam == null) { 
            //    response.ToFailedResponse("Campaign chưa được tạo", StatusCodes.Status400BadRequest);
            //    return response;
            //}

            //var listCandidate = await dbContext.CandidateProfiles
            //   .Where(p => p.CampaignId == campaignId).ToListAsync();
            //IEnumerable<GetListCandidateCampaignResponse> result = listCandidate.Select(
            //  x =>
            //  {
            //      return new GetListCandidateCampaignResponse()
            //      {
            //          CandidateProfileId = x.CandidateProfileId,
            //          NickName = x.NickName,
            //          Dob = x.Dob,
            //          Image = x.Image,
            //          UserName = x.UserName,
            //      };
            //  }
            //  ).ToList();
            //if(result==null)
            //{
            //    response.ToFailedResponse("Không có Candidate nào trong Campaign", StatusCodes.Status400BadRequest);
            //    return response;
            //}           
            //response.ToSuccessResponse(response.Data = result,"Lấy danh sách thành công", StatusCodes.Status200OK);
            //return response;
            return null;
        }

        public async Task<UpdateCandidateProfileResponse> UpdateCandidateProfile(Guid id, UpdateCandidateProfileRequesst request)
        {
            //var updateProfile = await dbContext.CandidateProfiles.Where(p => p.CandidateProfileId.Equals(id)).SingleOrDefaultAsync();
            //if (updateProfile == null)
            //{
            //    return null;
            //}
            //updateProfile.NickName = request.NickName;
            //updateProfile.Dob = request.Dob;
            //updateProfile.Image = request.Image;

            //var user = await dbContext.Users.Where(p => p.UserName.Equals(updateProfile.UserName)).SingleOrDefaultAsync();
            //user.Name = request.Name;
            //user.Gender = request.Gender;
            //user.Address = request.Address;
            //dbContext.CandidateProfiles.Update(updateProfile);
            //dbContext.Users.Add(user);
            //await dbContext.SaveChangesAsync();

            //var map = _mapper.Map<UpdateCandidateProfileResponse>(updateProfile);
            //map.Name = user.Name;
            //map.Gender = user.Gender;
            //map.Address = user.Address;

            //return map;
            return null;
        }
    }
}
