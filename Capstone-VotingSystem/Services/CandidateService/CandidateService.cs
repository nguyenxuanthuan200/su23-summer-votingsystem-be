using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CandidateRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActivityResponse;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Services.ActivityService;
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
        public async Task<APIResponse<string>> CreateAccountCandidateCampaign(CreateAccountCandidateRequest request)
        {
            APIResponse<string> response = new();

            var checkcam = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId && p.Status == true).SingleOrDefaultAsync();
            if (checkcam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkcam.Process != "Chưa bắt đầu")
            {
                response.ToFailedResponse("Không thể chỉnh sửa khi chiến dịch đang diễn ra", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkcam.IsApprove == true)
            {
                response.ToFailedResponse("Không thể chỉnh sửa khi chiến dịch đã được xác nhận", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.listAccountCandidate.Count() == 0 || request.listAccountCandidate == null)
            {
                response.ToFailedResponse("Danh sách tài khoản ứng cứ viên khởi tạo trống", StatusCodes.Status400BadRequest);
                return response;
            }
            foreach (var i in request.listAccountCandidate)
            {
                var check = await dbContext.Accounts.Where(p => p.UserName == i.UserName).SingleOrDefaultAsync();
                if (check != null)
                {
                    response.ToFailedResponse("Tài khoản " + i.UserName + " đã tồn tại", StatusCodes.Status400BadRequest);
                    return response;
                }
                var checkGroup = await dbContext.Groups.Where(p => p.GroupId == i.GroupId && p.IsVoter == false).SingleOrDefaultAsync();
                if (checkGroup == null)
                {
                    response.ToFailedResponse("Nhóm của ứng cử viên " + i.UserName + " không tồn tại", StatusCodes.Status400BadRequest);
                    return response;
                }
                var role = await dbContext.Roles.Where(p => p.Name.Equals("user")).SingleOrDefaultAsync();
                TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
                Account acc = new Account();
                {
                    acc.UserName = i.UserName;
                    acc.Password = i.Password;
                    acc.CreateAt = currentDateTimeVn;
                    acc.Status = true;
                    acc.RoleId = role.RoleId;
                };
                User us = new User();
                {
                    us.UserId = i.UserName;
                    us.FullName = i.FullName;
                    us.Status = true;
                    us.Permission = 0;
                }
                var id = Guid.NewGuid();
                Candidate candida = new Candidate();
                {
                    candida.CandidateId = id;
                    candida.UserId = us.UserId;
                    candida.FullName = i.FullName;
                    candida.Status = true;
                    candida.CampaignId = request.CampaignId;
                    candida.GroupId = i.GroupId;
                }
                await dbContext.Users.AddAsync(us);
                await dbContext.Accounts.AddAsync(acc);
                await dbContext.Candidates.AddAsync(candida);
                await dbContext.SaveChangesAsync();
            }
            response.ToSuccessResponse("Thêm ứng cử viên thành công", StatusCodes.Status200OK);
            return response;

        }

        public async Task<APIResponse<string>> CreateCandidateCampaign(CreateCandidateCampaignRequest request)
        {
            APIResponse<string> response = new();

            var checkCam = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId).SingleOrDefaultAsync();
            if (checkCam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa!!!!", StatusCodes.Status404NotFound);
                return response;
            }

            if (request.ListUser != null)
            {
                List<ListUserRequest> listUser = new List<ListUserRequest>();
                foreach (var i in request.ListUser)
                {
                    var checkuser = await dbContext.Users.Where(p => p.UserId == i.UserId && p.Status == true).SingleOrDefaultAsync();
                    if (checkuser == null)
                    {
                        response.ToFailedResponse("UserName " + i.UserId + "không tồn tại hoặc đã bị xóa!!", StatusCodes.Status404NotFound);
                        return response;
                    }

                    var check3 = await dbContext.Candidates.Where(p => p.CampaignId == request.CampaignId && p.UserId == i.UserId && p.Status == true).SingleOrDefaultAsync();
                    if (check3 != null)
                    {
                        response.ToFailedResponse("Candidate " + check3.UserId + " đã được thêm vào trước đó rồi!!!!", StatusCodes.Status400BadRequest);
                        return response;
                    }
                    var checkGroup = await dbContext.Groups.Where(p => p.GroupId == i.GroupId && p.IsVoter == false).SingleOrDefaultAsync();
                    if (checkGroup == null)
                    {
                        response.ToFailedResponse("Group ứng cử viên" + i.UserId + " không tồn tại", StatusCodes.Status400BadRequest);
                        return response;
                    }
                    TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                    DateTime currentDateTimeVn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTimeZone);
                    var check4 = await dbContext.Candidates.Where(p => p.CampaignId == request.CampaignId && p.UserId == i.UserId && p.Status == false).SingleOrDefaultAsync();
                    if (check4 != null)
                    {
                        check4.Status = true;
                        dbContext.Candidates.Update(check4);

                        Guid idNoti = Guid.NewGuid();
                        Notification noti = new Notification()
                        {
                            NotificationId = idNoti,
                            Title = "Thông báo chiến dịch",
                            Message = "Bạn vừa được thêm vào chiến dịch - " + checkCam.Title + " để làm ứng cử viên của chiến dịch đó, hãy kiểm tra thông tin cũng như chuẩn bị hồ sơ của mình để có được điểm số cao nhé!!!!!",
                            CreateDate = currentDateTimeVn,
                            IsRead = false,
                            Status = true,
                            Username = checkuser.UserId,
                            CampaignId = checkCam.CampaignId,
                        };
                        await dbContext.Notifications.AddAsync(noti);
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        var id = Guid.NewGuid();
                        Candidate can = new Candidate();
                        {
                            can.CandidateId = id;
                            can.UserId = i.UserId;
                            can.Status = true;
                            can.Description = i.Description;
                            can.CampaignId = request.CampaignId;
                            can.GroupId = i.GroupId;

                        }
                        await dbContext.Candidates.AddAsync(can);
                        Guid idNoti = Guid.NewGuid();
                        Notification noti = new Notification()
                        {
                            NotificationId = idNoti,
                            Title = "Thông báo chiến dịch",
                            Message = "Bạn vừa được thêm vào chiến dịch - " + checkCam.Title + " để làm ứng cử viên của chiến dịch đó, hãy kiểm tra thông tin cũng như chuẩn bị hồ sơ của mình để có được điểm số cao nhé!!!!!",
                            CreateDate = currentDateTimeVn,
                            IsRead = false,
                            Status = true,
                            Username = checkuser.UserId,
                            CampaignId = checkCam.CampaignId,
                        };
                        await dbContext.Notifications.AddAsync(noti);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            else
            {
                response.ToFailedResponse("Danh sách ứng cử viên trống!!", StatusCodes.Status400BadRequest);
                return response;
            }

            response.ToSuccessResponse("Thêm danh sách ứng cử viên thành công.", StatusCodes.Status200OK);
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
               .Where(p => p.CampaignId == campaignId && p.Status == true).ToListAsync();
            List<GetListCandidateCampaignResponse> result = new List<GetListCandidateCampaignResponse>();
            foreach (var item in listCandidate)
            {
                var checkuser = await dbContext.Users.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.UserId == item.UserId);
                if (checkuser != null)
                {
                    var checkGroup = await dbContext.Groups.Where(p => p.GroupId == item.GroupId).SingleOrDefaultAsync();
                    var candidate = new GetListCandidateCampaignResponse();
                    {
                        candidate.CandidateId = item.CandidateId;
                        candidate.CampaignId = item.CampaignId;
                        candidate.Description = item.Description;
                        candidate.UserId = item.UserId;
                        candidate.GroupId = checkGroup != null ? checkGroup.GroupId : null;
                        candidate.FullName = checkuser.FullName;
                        candidate.Phone = checkuser.Phone;
                        candidate.Status = checkuser.Status;
                        candidate.Gender = checkuser.Gender;
                        candidate.Dob = checkuser.Dob;
                        candidate.Email = checkuser.Email;
                        candidate.AvatarUrl = checkuser.AvatarUrl;
                    }
                    result.Add(candidate);
                }
            }
            if (result == null)
            {
                response.ToFailedResponse("Không có Candidate nào trong Campaign", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        //public async Task<APIResponse<IEnumerable<GetListCandidateCampaignResponse>>> GetAllCandidate()
        //{
        //    APIResponse<IEnumerable<GetListCandidateCampaignResponse>> response = new();
        //    var listCandidate = await dbContext.Candidates.Where(p => p.Status == true).ToListAsync();
        //    List<GetListCandidateCampaignResponse> result = new List<GetListCandidateCampaignResponse>();
        //    foreach (var item in listCandidate)
        //    {
        //        var checkuser = await dbContext.Users.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.UserId == item.UserId);
        //        var candidate = new GetListCandidateCampaignResponse();
        //        {
        //            candidate.CandidateId = item.CandidateId;
        //            candidate.CampaignId = item.CampaignId;
        //            candidate.Description = item.Description;
        //            candidate.UserId = item.UserId;
        //            candidate.GroupId = item.GroupId;
        //            candidate.FullName = checkuser.FullName;
        //            candidate.Phone = checkuser.Phone;
        //            candidate.Gender = checkuser.Gender;
        //            candidate.Dob = checkuser.Dob;
        //            candidate.Email = checkuser.Email;
        //            candidate.AvatarUrl = checkuser.AvatarUrl;
        //        }
        //        var checkcam = await dbContext.Campaigns.Where(p => p.Status == true && p.CampaignId == item.CampaignId).SingleOrDefaultAsync();
        //        if (checkcam != null)
        //            result.Add(candidate);
        //    }
        //    if (result == null)
        //    {
        //        response.ToFailedResponse("Không có Candidate nào trong Campaign", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
        //    return response;
        //}

        public async Task<APIResponse<string>> DeleteCandidateCampaign(Guid candidateId, DeleteCandidateRequest request)
        {
            APIResponse<string> response = new();
            var checkCampaign = await dbContext.Campaigns.Where(p => p.Status == true && p.UserId == request.userId && p.CampaignId == request.campaignId).SingleOrDefaultAsync();
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Không phải là người tạo chiến dịch, bạn không thể xóa ứng cử viên trong đó hoặc chiến dịch đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var deleteCandidate = await dbContext.Candidates.Where(p => p.Status == true && p.CandidateId == candidateId && p.CampaignId == request.campaignId).SingleOrDefaultAsync();
            if (deleteCandidate == null)
            {
                response.ToFailedResponse("Không có Candidate nào phù hợp trong Campaign hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            deleteCandidate.Status = false;
            dbContext.Candidates.Update(deleteCandidate);
            await dbContext.SaveChangesAsync();
            response.ToSuccessResponse("Loại bỏ ứng cử viên thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GetCandidateDetailResponse>> GetCandidateById(Guid candidateId, Guid stageId)
        {
            APIResponse<GetCandidateDetailResponse> response = new();
            var checkcandi = await dbContext.Candidates.Where(p => p.CandidateId == candidateId && p.Status == true).SingleOrDefaultAsync();
            if (checkcandi == null)
            {
                response.ToFailedResponse("Ứng cử viên không tồn tại hoặc đã bị xóa!!", StatusCodes.Status404NotFound);
                return response;
            }
            var checkuser = await dbContext.Users.Where(p => p.UserId == checkcandi.UserId && p.Status == true).SingleOrDefaultAsync();
            if (checkuser == null)
            {
                response.ToFailedResponse("Candidate không tồn tại hoặc đã bị xóa!!", StatusCodes.Status404NotFound);
                return response;
            }
            var checkCampaign = await dbContext.Stages.Where(p => p.StageId == stageId && p.Status == true).SingleOrDefaultAsync();
            if (checkCampaign == null)
            {
                response.ToFailedResponse("Giai đoạn không tồn tại hoặc đã bị xóa!!", StatusCodes.Status404NotFound);
                return response;
            }
            var scoreStage = await dbContext.Scores.Where(p => p.StageId == stageId && p.CandidateId == candidateId).SingleOrDefaultAsync();
            var score = 0;
            if (scoreStage != null)
            {
                score = (int)scoreStage.Point;
            }
            var map = _mapper.Map<GetCandidateDetailResponse>(checkuser);
            map.CandidateId = checkcandi.CandidateId;
            map.Description = checkcandi.Description;
            map.StageId = stageId;
            map.CampaignId = checkCampaign.CampaignId;
            map.Score = score;
            //Get Activity
            #region
            var listActivity = new List<GetActivityByCandidateResponse>();
            var activity = new GetActivityByCandidateResponse();

            var listContent = new List<GetActivityContentResponse>();
            var content = new GetActivityContentResponse();

            var checkCandidate = await dbContext.Candidates.Where(p => p.CandidateId == candidateId && p.Status == true).SingleOrDefaultAsync();
            if (checkCandidate == null)
            {
                response.ToFailedResponse("Ứng cử viên không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkAccount = await dbContext.Users.Where(p => p.UserId == checkCandidate.UserId).SingleOrDefaultAsync();
            if (checkAccount == null || checkAccount.Status == false)
            {
                response.ToFailedResponse("Ứng cử viên không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }

            var getActivityContent = await dbContext.ActivityContents.Where(p => p.CandidateId == candidateId)
                .ToListAsync();
            if (getActivityContent == null || getActivityContent.Count <= 0)
            {
                response.ToFailedResponse(" Không có hoạt động nào tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }

            var getActivity = await dbContext.Activities.ToListAsync();
            foreach (var i in getActivity)
            {
                activity = new();
                listContent = new();
                foreach (var item in getActivityContent)
                {

                    if (i.ActivityId == item.ActivityId)
                    {
                        content = new();
                        content.ActivityContentId = item.ActivityContentId;
                        content.Content = item.Content;

                        listContent.Add(content);
                    }
                }
                if (listContent.Count > 0)
                {
                    activity.ListContent = listContent;
                    activity.Title = i.Title;
                    activity.ActivityId = i.ActivityId;
                    listActivity.Add(activity);
                }

            }
            #endregion
            map.ListActivity = listActivity;
            response.ToSuccessResponse("Xem chi tiết ứng cử viên thành công!!", StatusCodes.Status200OK);
            response.Data = map;
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

        public async Task<APIResponse<GetListCandidateStageResponse>> GetListCandidatStage(Guid stageId, string userId)
        {
            APIResponse<GetListCandidateStageResponse> response = new APIResponse<GetListCandidateStageResponse>();
            var checkStage = await dbContext.Stages.Where(p => p.StageId == stageId && p.Status == true).SingleOrDefaultAsync();
            if (checkStage == null)
            {
                response.ToFailedResponse("Giai đoạn không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkcam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.CampaignId == checkStage.CampaignId);
            if (checkcam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var stage = new GetListCandidateStageResponse()
            {
                StageId = stageId,
                CampaignId = checkStage.CampaignId,
                FormId = checkStage.FormId,
                StageName = checkStage.Title,
                CampaignName = checkcam.Title,
            };
            List<ListCandidateVotedByUser> listVoted = new();
            var checkVote = await dbContext.Votings.Where(p => p.StageId == stageId && p.UserId == userId && p.Status == true).ToListAsync();

            var listCandidate = await dbContext.Candidates.Where(p => p.Status == true && p.CampaignId == checkcam.CampaignId).ToListAsync();
            List<ListCandidateStageResponse> result = new List<ListCandidateStageResponse>();
            foreach (var item in listCandidate)
            {

                var checkuser = await dbContext.Users.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.UserId == item.UserId);
                if (checkuser != null || item.UserId == null)
                {
                    var checkCandidate = await dbContext.Candidates.Where(p => p.Status == true).SingleOrDefaultAsync(p => p.UserId == item.UserId && p.CampaignId == checkcam.CampaignId);
                    var group = await dbContext.Groups.SingleOrDefaultAsync(p => p.GroupId == item.GroupId);
                    var scoreStage = await dbContext.Scores.Where(p => p.StageId == stage.StageId && p.CandidateId == item.CandidateId).SingleOrDefaultAsync();
                    var score = 0;
                    if (scoreStage != null)
                    {
                        score = (int)scoreStage.Point;
                    }
                    bool voted = false;
                    if (checkVote != null)
                    {

                        foreach (var a in checkVote)
                        {
                            if (a.CandidateId == item.CandidateId)
                                voted = true;

                        }
                    }
                    var candidate = new ListCandidateStageResponse();
                    {
                        candidate.CandidateId = item.CandidateId;
                        candidate.Description = item.Description;
                        candidate.UserId = item.UserId;
                        candidate.GroupId = item.GroupId;
                        candidate.GroupName = group != null ? group.Name : null;
                        candidate.FullName = item.FullName;
                        candidate.Phone = checkuser != null ? checkuser.Phone : null;
                        candidate.Gender = checkuser != null ? checkuser.Gender : null;
                        candidate.Dob = checkuser != null ? checkuser.Dob : null;
                        candidate.Email = checkuser != null ? checkuser.Email : null;
                        candidate.AvatarUrl = item.AvatarUrl;
                        candidate.StageScore = score;
                        candidate.isVoted = voted;
                        //Get Activity
                        #region
                        var listActivity = new List<GetActivityByCandidateResponse>();
                        var activity = new GetActivityByCandidateResponse();

                        var listContent = new List<GetActivityContentResponse>();
                        var content = new GetActivityContentResponse();

                        var getActivityContent = await dbContext.ActivityContents.Where(p => p.CandidateId == item.CandidateId)
                            .ToListAsync();
                        var getActivity = await dbContext.Activities.ToListAsync();
                        foreach (var i in getActivity)
                        {
                            activity = new();
                            listContent = new();
                            foreach (var itemContent in getActivityContent)
                            {

                                if (i.ActivityId == itemContent.ActivityId)
                                {
                                    content = new();
                                    content.ActivityContentId = itemContent.ActivityContentId;
                                    content.Content = itemContent.Content;

                                    listContent.Add(content);
                                }
                            }
                            if (listContent.Count > 0)
                            {
                                activity.ListContent = listContent;
                                activity.Title = i.Title;
                                activity.ActivityId = i.ActivityId;
                                listActivity.Add(activity);
                            }

                        }
                        #endregion
                        candidate.ListActivity = listActivity;
                    }
                    result.Add(candidate);
                }

            }
            var VoteRemaining = await checkVoteRemaining(userId, stageId);

            stage.votesRemaining = VoteRemaining;
            stage.Candidate = result;
            response.Data = stage;

            if (response.Data == null)
            {
                response.ToFailedResponse("Không có ứng cử viên nào", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }

        private async Task<GetVoteResponse> checkVoteRemaining(string userId, Guid stageid)
        {
            var result = new GetVoteResponse();
            var checkVote = await dbContext.Votings.Where(p => p.UserId == userId && p.StageId == stageid && p.Status == true).ToListAsync();


            var getStage = await dbContext.Stages.Where(p => p.StageId == stageid && p.Status == true).SingleOrDefaultAsync();

            int VoteBM = 0;
            int VoteAM = 0;

            Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
            if (getStage.CampaignId == cam)
            {
                var groupNames = dbContext.Users.Where(u => u.UserId == userId).Join(dbContext.GroupUsers, u => u.UserId, ug => ug.UserId, (u, ug) => ug.GroupId)
          .Join(dbContext.Groups, gid => gid, g => g.GroupId, (gid, g) => g.Name);

                foreach (var item in groupNames)
                {
                    if (item.Equals("Chuyên ngành (1 - 6)"))
                    {
                        VoteBM = 1;
                        VoteAM = 2;
                    }
                    else if (item.Equals("Chuyên ngành (7 - 9)"))
                    {
                        VoteBM = 0;
                        VoteAM = 3;
                    }
                    else
                    {
                        VoteBM = 3;
                        VoteAM = 0;
                    }

                }

                foreach (var item in checkVote)
                {
                    var groupGroup = dbContext.Votings.Where(u => u.CandidateId == item.CandidateId && u.Status == true).Join(dbContext.Candidates, u => u.CandidateId, ug => ug.CandidateId, (u, ug) => ug.GroupId)
          .Join(dbContext.Groups, gid => gid, g => g.GroupId, (gid, g) => g.Name);
                    if (item.Equals("Nhạc cụ dân tộc") || item.Equals("Giáo dục thể chất") || item.Equals("Tiếng anh dự bị"))
                    {
                        VoteBM--;
                    }
                    else
                    {

                        VoteAM--;
                    }
                }
            }
            result.voteAM = VoteAM;
            result.voteBM = VoteBM;
            result.voteRemaining = getStage.LimitVote - checkVote.Count();


            return result;

        }

        public async Task<APIResponse<string>> CreateListCandidate(CreateListCandidateRequest request)
        {
            APIResponse<string> response = new();

            var checkcam = await dbContext.Campaigns.Where(p => p.CampaignId == request.CampaignId && p.Status == true).SingleOrDefaultAsync();
            if (checkcam == null)
            {
                response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkcam.Process != "Chưa bắt đầu")
            {
                response.ToFailedResponse("Không thể chỉnh sửa khi chiến dịch đang diễn ra", StatusCodes.Status400BadRequest);
                return response;
            }
            if (checkcam.IsApprove == true)
            {
                response.ToFailedResponse("Không thể chỉnh sửa khi chiến dịch đã được xác nhận", StatusCodes.Status400BadRequest);
                return response;
            }
            if (request.listCandidate.Count() == 0 || request.listCandidate == null)
            {
                response.ToFailedResponse("Danh sách tài khoản ứng cứ viên khởi tạo trống", StatusCodes.Status400BadRequest);
                return response;
            }
            foreach (var i in request.listCandidate)
            {
                var checkGroup = await dbContext.Groups.Where(p => p.Name.ToUpper().Equals(i.GroupName.ToUpper()) && p.IsVoter == false).SingleOrDefaultAsync();
                if (checkGroup == null)
                {
                    response.ToFailedResponse("Nhóm của ứng cử viên " + i.FullName + " không tồn tại trong chiến dịch. Vui lòng kiểm tra lại!!", StatusCodes.Status400BadRequest);
                    return response;
                }
                var id = Guid.NewGuid();
                Candidate candida = new Candidate();
                {
                    candida.CandidateId = id;
                    //  candida.UserId = us.UserId;
                    candida.FullName = i.FullName;
                    candida.Status = true;
                    candida.CampaignId = request.CampaignId;
                    candida.GroupId = checkGroup.GroupId;
                }
                await dbContext.Candidates.AddAsync(candida);
                await dbContext.SaveChangesAsync();
            }
            response.ToSuccessResponse("Thêm ứng cử viên thành công", StatusCodes.Status200OK);
            return response;
        }
    }

}
