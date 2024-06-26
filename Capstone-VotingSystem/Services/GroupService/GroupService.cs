﻿using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.GroupRequest;
using Capstone_VotingSystem.Models.ResponseModels.GroupResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Services.GroupService
{
    public class GroupService : IGroupService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public GroupService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            _mapper = mapper;
        }

        public async Task<APIResponse<string>> CheckGroupUser(string userName, Guid campaignId)
        {
            APIResponse<string> response = new();

            var checkUser = await dbContext.Users.Where(x => x.UserId == userName && x.Status == true).SingleOrDefaultAsync();
            if (checkUser == null)
            {
                response.ToFailedResponse("Tài khoản này không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var checkGroup = await dbContext.GroupUsers.Where(x => x.UserId == userName && x.CampaignId == campaignId).ToListAsync();

            foreach (var item in checkGroup)
            {
                var group = await dbContext.Groups.Where(p => p.GroupId == item.GroupId && p.IsVoter == true && p.IsStudentMajor == false).SingleOrDefaultAsync();
                if (group != null)
                {
                    response.ToSuccessResponse("Đã chọn nhóm cho chiến dịch này", StatusCodes.Status200OK);
                    return response;
                }
            }
            response.ToFailedResponse("Chưa chọn nhóm cho chiến dịch này", StatusCodes.Status400BadRequest);
            return response;
        }

        public async Task<APIResponse<GroupResponse>> CreateGroup(CreateGroupRequest request)
        {
            APIResponse<GroupResponse> response = new();
            var checkCam = await dbContext.Campaigns.Where(x => x.CampaignId == request.CampaignId && x.Status == true).SingleOrDefaultAsync();
            if (checkCam == null)
            {
                response.ToFailedResponse("Chiến dịch này không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            if (checkCam.IsApprove == true)
            {
                response.ToFailedResponse("Không thể thay đổi khi đã xác nhận điều khoản", StatusCodes.Status404NotFound);
                return response;
            }
            var checkGroup = await dbContext.Groups.Where(x => x.Name.ToUpper().Equals(request.Name.ToUpper()) && x.CampaignId == request.CampaignId).SingleOrDefaultAsync();
            if (checkGroup != null)
            {
                response.ToFailedResponse("Tên của nhóm đã tồn tại trong chiến dịch này rồi", StatusCodes.Status400BadRequest);
                return response;
            }
            bool studentMajor = false;
            Guid cam = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
            if (request.IsStudentMajor != null && checkCam.CampaignId == cam)
            {
                studentMajor = request.IsStudentMajor ?? false;
            }
            var id = Guid.NewGuid();
            Group gr = new Group();
            {
                gr.GroupId = id;
                gr.Name = request.Name;
                gr.Description = request.Description;
                gr.IsVoter = request.IsVoter;
                gr.IsStudentMajor = studentMajor;
                gr.CampaignId = request.CampaignId;
            }
            await dbContext.Groups.AddAsync(gr);
            await dbContext.SaveChangesAsync();
            if (gr.IsVoter == true && gr.IsStudentMajor == false)
            {
                var listGroup = await dbContext.Groups.Where(p => p.IsVoter == false && p.CampaignId == checkCam.CampaignId).ToListAsync();
                if (listGroup.Count > 0)
                {
                    foreach (var group in listGroup)
                    {
                        var idRa = Guid.NewGuid();
                        Ratio ratio = new Ratio();
                        {
                            ratio.RatioGroupId = idRa;
                            ratio.Proportion = 1;
                            ratio.GroupVoterId = gr.GroupId;
                            ratio.CampaignId = checkCam.CampaignId;
                            ratio.GroupCandidateId = group.GroupId;
                        };
                        await dbContext.Ratios.AddAsync(ratio);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            else if (gr.IsVoter == false)
            {
                var listGroup = await dbContext.Groups.Where(p => p.IsVoter == true && p.CampaignId == checkCam.CampaignId).ToListAsync();
                if (listGroup.Count > 0)
                {
                    foreach (var group in listGroup)
                    {
                        var idRa = Guid.NewGuid();
                        Ratio ratio = new Ratio();
                        {
                            ratio.RatioGroupId = idRa;
                            ratio.Proportion = 1;
                            ratio.GroupVoterId = group.GroupId;
                            ratio.CampaignId = checkCam.CampaignId;
                            ratio.GroupCandidateId = gr.GroupId;
                        };
                        await dbContext.Ratios.AddAsync(ratio);

                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            var map = _mapper.Map<GroupResponse>(gr);
            response.ToSuccessResponse("Tạo nhóm thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        //public async Task<APIResponse<string>> DeleteGroup(Guid groupId)
        //{
        //    APIResponse<string> response = new();
        //    var checkGroup = await dbContext.Groups.Where(p => p.GroupId == groupId).SingleOrDefaultAsync();
        //    if (checkGroup == null)
        //    {
        //        response.ToFailedResponse("Nhóm không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    var checkCam = await dbContext.Campaigns.Where(p => p.CampaignId == checkGroup.CampaignId && p.Status == true).SingleOrDefaultAsync();
        //    if (checkCam == null)
        //    {
        //        response.ToFailedResponse("Chiến dịch không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    if (checkCam.IsApporve == true)
        //    {
        //        response.ToFailedResponse("Không thể xóa nhóm khi chiến dịch đã được duyệt", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    var checkCandidate = await dbContext.Candidates.Where(p => p.GroupId == groupId).SingleOrDefaultAsync();
        //    if (checkGroup == null)
        //    {
        //        response.ToFailedResponse("Nhóm không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
        //        return response;
        //    }
        //    dbContext.Candidates.Update(deleteCandidate);
        //    await dbContext.SaveChangesAsync();
        //    response.ToSuccessResponse("Xóa Candidate thành công", StatusCodes.Status200OK);
        //    return response;
        //}



        public async Task<APIResponse<IEnumerable<GroupResponse>>> GetListGroupByCampaign(Guid campaignId)
        {
            APIResponse<IEnumerable<GroupResponse>> response = new();
            var type = await dbContext.Groups.Where(p => p.CampaignId == campaignId).ToListAsync();
            IEnumerable<GroupResponse> result = type.Select(x =>
            {
                return new GroupResponse()
                {
                    GroupId = x.GroupId,
                    Name = x.Name,
                    Description = x.Description,
                    IsVoter = x.IsVoter,
                    IsStudentMajor = x.IsStudentMajor,

                };
            }).ToList();
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách nhóm cho người bình chọn thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<StatisticalGroupResponse>> StatisticalGroupByCampaign(Guid campaignId)
        {
            APIResponse<StatisticalGroupResponse> response = new();
            var result = new StatisticalGroupResponse();
            var listVoter = new List<StatisticalVoterInGroupResponse>();
            var listVoterMajor = new List<StatisticalVoterMajorInGroupResponse>();
            var listCandidate = new List<StatisticalCandidateInGroupResponse>();
            var campaign = await dbContext.Campaigns.Where(p => p.CampaignId == campaignId && p.Status == true && p.IsApprove == true).SingleOrDefaultAsync();
            if (campaign == null)
            {
                response.ToFailedResponse("Không thể thống kê chiến dịch đã bị xóa hoặc chưa cam kết ", StatusCodes.Status400BadRequest);
                return response;
            }
            var groupVoter = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == true).ToListAsync();
            if (groupVoter.Count() == 0)
            {
                response.ToFailedResponse("Không có nhóm cho người bình chọn nào trong chiến dịch tồn tại", StatusCodes.Status404NotFound);
                return response;
            }
            int totalVoterInCampaign = 0;
            Guid camCtsv = Guid.Parse("6097a517-11ad-4105-b26a-0e93bea2cb43");
            if (campaign.CampaignId == camCtsv)
            {
                var groupVoterMajor = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == true && p.IsStudentMajor == true).ToListAsync();
                if (groupVoterMajor.Count() == 0)
                {
                    response.ToFailedResponse("Không có nhóm ngành của sinh viên nào trong chiến dịch tồn tại", StatusCodes.Status404NotFound);
                    return response;
                }
                foreach (var i in groupVoterMajor)
                {
                    var ListVoterGroup = new StatisticalVoterMajorInGroupResponse();
                    ListVoterGroup.GroupId = i.GroupId;
                    ListVoterGroup.GroupName = i.Name;
                    var groupUser = await dbContext.GroupUsers.Where(p => p.CampaignId == campaignId && p.GroupId == i.GroupId).ToListAsync();
                    ListVoterGroup.TotalVoterMajorInGroup = groupUser.Count();
                    listVoterMajor.Add(ListVoterGroup);
                }


                foreach (var i in groupVoter)
                {
                    if (i.IsStudentMajor == false)
                    {
                        var ListVoterGroup = new StatisticalVoterInGroupResponse();
                        ListVoterGroup.GroupId = i.GroupId;
                        ListVoterGroup.GroupName = i.Name;
                        var groupUser = await dbContext.GroupUsers.Where(p => p.CampaignId == campaignId && p.GroupId == i.GroupId).ToListAsync();
                        ListVoterGroup.TotalVoterInGroup = groupUser.Count();
                        listVoter.Add(ListVoterGroup);
                        totalVoterInCampaign += ListVoterGroup.TotalVoterInGroup;
                    }
                }

            }
            else
            {
                foreach (var i in groupVoter)
                {
                    var ListVoterGroup = new StatisticalVoterInGroupResponse();
                    ListVoterGroup.GroupId = i.GroupId;
                    ListVoterGroup.GroupName = i.Name;
                    var groupUser = await dbContext.GroupUsers.Where(p => p.CampaignId == campaignId && p.GroupId == i.GroupId).ToListAsync();
                    ListVoterGroup.TotalVoterInGroup = groupUser.Count();
                    listVoter.Add(ListVoterGroup);
                    totalVoterInCampaign += ListVoterGroup.TotalVoterInGroup;

                }
            }

            var groupCandidate = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == false).ToListAsync();
            if (groupCandidate.Count() == 0)
            {
                response.ToFailedResponse("Không có nhóm cho người ứng cử nào trong chiến dịch tồn tại", StatusCodes.Status404NotFound);
                return response;
            }

            int totalCandidateInCampaign = 0;
            foreach (var i in groupCandidate)
            {
                var ListCandidateGroup = new StatisticalCandidateInGroupResponse();
                ListCandidateGroup.GroupId = i.GroupId;
                ListCandidateGroup.GroupName = i.Name;
                var groupUser = await dbContext.Candidates.Where(p => p.CampaignId == campaignId && p.GroupId == i.GroupId).ToListAsync();
                ListCandidateGroup.TotalCandidateInGroup = groupUser.Count();
                listCandidate.Add(ListCandidateGroup);
                totalCandidateInCampaign += ListCandidateGroup.TotalCandidateInGroup;
            }
            result.TotalVoterInCampaign = totalVoterInCampaign;
            result.TotalCandiadteInCampaign = totalCandidateInCampaign;
            result.StatisticalVoterInGroup = listVoter;
            result.StatisticalVoterMajorInGroup = listVoterMajor;
            result.StatisticalCandidateInGroup = listCandidate;
            response.ToSuccessResponse(response.Data = result, "Thống kê danh sách nhóm cho người bình chọn thành công", StatusCodes.Status200OK);
            response.Data = result;
            return response;
        }

        public async Task<APIResponse<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request)
        {
            APIResponse<GroupResponse> response = new();
            var groupCheck = await dbContext.Groups.SingleOrDefaultAsync(c => c.GroupId == id);
            if (groupCheck == null)
            {
                response.ToFailedResponse("Nhóm không tồn tại hoặc đã bị xóa", StatusCodes.Status404NotFound);
                return response;
            }
            var typeNameCheck = await dbContext.Groups.SingleOrDefaultAsync(c => c.Name.ToUpper().Trim().Equals(request.Name.ToUpper().Trim()) && c.CampaignId == groupCheck.CampaignId);
            if (typeNameCheck != null)
            {
                response.ToFailedResponse("Tên nhóm đã tồn tại trong chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }


            groupCheck.Name = request.Name;
            groupCheck.Description = request.Description;
            //groupCheck.IsVoter = request.IsVoter;
            dbContext.Groups.Update(groupCheck);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GroupResponse>(groupCheck);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
