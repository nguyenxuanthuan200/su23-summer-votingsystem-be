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
            var checkcam = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId && p.Status!=false).SingleOrDefaultAsync();
            if (checkcam == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var role = await dbContext.Roles.Where(p => p.Name.Equals("user")).SingleOrDefaultAsync();
            Account acc = new Account();
            {
                acc.UserName = request.UserName;
                acc.Password = request.Password;
                acc.CreateAt= DateTime.Now;
                acc.Status = true;
                acc.RoleId = role.RoleId;
            };
            var checkGroup = await dbContext.Groups.Where(p => p.GroupId == request.GroupId).SingleOrDefaultAsync();
            if (checkGroup == null)
            {
                response.ToFailedResponse("Group không tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            
            User us = new User();
            {
                us.UserId = request.UserName;
                us.FullName = request.FullName;
                us.Status = true;
                us.Address = request.Address;
                us.GroupId = request.GroupId;
            }
            var id = Guid.NewGuid();
            Candidate candida = new Candidate();
            {
                candida.CandidateId = id;
                candida.UserId = us.UserId;
                candida.Status = true;
                candida.CampaignId = request.CampaignId;
            }
            await dbContext.Users.AddAsync(us);
            await dbContext.Accounts.AddAsync(acc);
            await dbContext.Candidates.AddAsync(candida);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateAccountCandidateResponse>(us);
            response.ToSuccessResponse("Tạo thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;

        }

        public async Task<APIResponse<CreateCandidateCampaignResponse>> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            APIResponse<CreateCandidateCampaignResponse> response = new();
            var checkuser = await dbContext.Users.Where(p => p.UserId == request.UserId).SingleOrDefaultAsync();
            if (checkuser == null)
            {
                response.ToFailedResponse("UserName không tồn tại hoặc đã bị xóa!!", StatusCodes.Status404NotFound);
                return response;
            }
            var check2 = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId).SingleOrDefaultAsync();
            if (check2 == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa!!!!", StatusCodes.Status404NotFound);
                return response;
            }
            var check3 = await dbContext.Candidates.Where(p => p.CampaignId == request.CampaignId && p.UserId==request.UserId).SingleOrDefaultAsync();
            if (check3 != null)
            {
                response.ToFailedResponse("Candidate đã được thêm vào trước đó rồi!!!!", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Candidate can = new Candidate();
            {
                can.CandidateId = id;
                can.UserId = request.UserId;
                can.Status = true;
                can.Description = request.Description;
                can.CampaignId = request.CampaignId;
            }

            await dbContext.Candidates.AddAsync(can);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateCandidateCampaignResponse>(can);
            response.ToSuccessResponse("Thêm Candidate Thành công!!", StatusCodes.Status200OK);
            response.Data = map;
            return response;

        }

 
        public async Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetListCandidateCampaign(Guid campaignId)
        {
            APIResponse<IEnumerable<GetListCandidateCampaignResponse>> response = new();
            var checkcam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.CampaignId == campaignId);
            if (checkcam == null)
            {
                response.ToFailedResponse("Campaign không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }

            var listCandidate = await dbContext.Candidates
               .Where(p => p.CampaignId == campaignId).ToListAsync();
            List<GetListCandidateCampaignResponse> result = new List<GetListCandidateCampaignResponse>();
            foreach (var item in listCandidate)
            {
                var checkuser = await dbContext.Users.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.UserId == item.UserId);
                var candidate = new GetListCandidateCampaignResponse();
                {
                    candidate.CandidateId = item.CandidateId;
                    candidate.CampaignId = item.CampaignId;
                    candidate.Description = item.Description;
                    candidate.UserId = item.UserId;
                    candidate.GroupId = checkuser.GroupId;
                    candidate.FullName = checkuser.FullName;
                    candidate.Phone = checkuser.Phone;
                    candidate.Gender = checkuser.Gender;
                    candidate.Dob = checkuser.Dob;
                    candidate.Email = checkuser.Email;
                    candidate.AvatarUrl = checkuser.AvatarUrl;
                }
                result.Add(candidate);
            }
            if (result == null)
            {
                response.ToFailedResponse("Không có Candidate nào trong Campaign", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetAllCandidate()
        {
            APIResponse<IEnumerable<GetListCandidateCampaignResponse>> response = new();
            var listCandidate = await dbContext.Candidates.Where(p => p.Status == true).ToListAsync();
            List<GetListCandidateCampaignResponse> result = new List<GetListCandidateCampaignResponse>();
            foreach (var item in listCandidate)
            {
                var checkuser = await dbContext.Users.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.UserId == item.UserId);
                var candidate = new GetListCandidateCampaignResponse();
                {
                    candidate.CandidateId = item.CandidateId;
                    candidate.CampaignId = item.CampaignId;
                    candidate.Description = item.Description;
                    candidate.UserId = item.UserId;
                    candidate.GroupId = checkuser.GroupId;
                    candidate.FullName = checkuser.FullName;
                    candidate.Phone = checkuser.Phone;
                    candidate.Gender = checkuser.Gender;
                    candidate.Dob = checkuser.Dob;
                    candidate.Email = checkuser.Email;
                    candidate.AvatarUrl = checkuser.AvatarUrl;
                }
                var checkcam = await dbContext.Campaigns.Where(p => p.Status == true&&p.CampaignId==item.CampaignId).SingleOrDefaultAsync();
                if(checkcam!=null)
                result.Add(candidate);
            }
            if (result == null)
            {
                response.ToFailedResponse("Không có Candidate nào trong Campaign", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<string>> DeleteCandidateCampaign(Guid candidateId, Guid campaignId, string userId)
        {
            APIResponse<string> response = new();
            var deleteCandidate = await dbContext.Candidates.Where(p =>p.Status==true&& p.CandidateId.Equals(candidateId)&&p.CampaignId==campaignId&&p.UserId.Equals(userId)).SingleOrDefaultAsync();
            if (deleteCandidate == null)
            {
                response.ToFailedResponse("Không có Candidate nào phù hợp trong Campaign hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            deleteCandidate.Status = false;
            dbContext.Candidates.Update(deleteCandidate);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Xóa Candidate thành công", StatusCodes.Status200OK);
            return response;
        }

        //public async Task<UpdateCandidateProfileResponse> UpdateCandidateProfile(Guid id, UpdateCandidateProfileRequesst request)
        //{
        //    //var updateProfile = await dbContext.CandidateProfiles.Where(p => p.CandidateProfileId.Equals(id)).SingleOrDefaultAsync();
        //    //if (updateProfile == null)
        //    //{
        //    //    return null;
        //    //}
        //    //updateProfile.NickName = request.NickName;
        //    //updateProfile.Dob = request.Dob;
        //    //updateProfile.Image = request.Image;

        //    //var user = await dbContext.Users.Where(p => p.UserName.Equals(updateProfile.UserName)).SingleOrDefaultAsync();
        //    //user.Name = request.Name;
        //    //user.Gender = request.Gender;
        //    //user.Address = request.Address;
        //    //dbContext.CandidateProfiles.Update(updateProfile);
        //    //dbContext.Users.Add(user);
        //    //await dbContext.SaveChangesAsync();

        //    //var map = _mapper.Map<UpdateCandidateProfileResponse>(updateProfile);
        //    //map.Name = user.Name;
        //    //map.Gender = user.Gender;
        //    //map.Address = user.Address;

        //    //return map;
        //    return null;
        //}
    }
}
