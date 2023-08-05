using AutoMapper;
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

            var checkGroup = await dbContext.GroupUsers.Where(x => x.UserId == userName && x.CampaignId == campaignId).SingleOrDefaultAsync();
            if (checkGroup == null)
            {
                response.ToFailedResponse("Chưa chọn nhóm cho chiến dịch này", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse("Đã chọn nhóm cho chiến dịch này", StatusCodes.Status200OK);
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
            var checkGroup = await dbContext.Groups.Where(x => x.Name.ToUpper().Equals(request.Name.ToUpper()) && x.CampaignId == request.CampaignId).SingleOrDefaultAsync();
            if (checkGroup != null)
            {
                response.ToFailedResponse("Tên của nhóm đã tồn tại trong chiến dịch này rồi", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Group gr = new Group();
            {
                gr.GroupId = id;
                gr.Name = request.Name;
                gr.Description = request.Description;
                gr.IsVoter = request.IsVoter;
                gr.CampaignId = request.CampaignId;

            }
            await dbContext.Groups.AddAsync(gr);
            await dbContext.SaveChangesAsync();
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

        public async Task<APIResponse<IEnumerable<GroupResponse>>> GetListGroup()
        {
            APIResponse<IEnumerable<GroupResponse>> response = new();
            var type = await dbContext.Groups.ToListAsync();
            IEnumerable<GroupResponse> result = type.Select(x =>
            {
                return new GroupResponse()
                {
                    GroupId = x.GroupId,
                    Name = x.Name,
                    Description = x.Description,
                    IsVoter = x.IsVoter,

                };
            }).ToList();
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách Group thành công", StatusCodes.Status200OK);
            return response;
        }

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
            var listCandidate = new List<StatisticalCandidateInGroupResponse>();
            var campaign = await dbContext.Campaigns.Where(p => p.CampaignId == campaignId && p.Status == true && p.IsApprove==true).SingleOrDefaultAsync();
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
            var groupCandidate = await dbContext.Groups.Where(p => p.CampaignId == campaignId && p.IsVoter == false).ToListAsync();
            if (groupCandidate.Count() == 0)
            {
                response.ToFailedResponse("Không có nhóm cho người ứng cử nào trong chiến dịch tồn tại", StatusCodes.Status404NotFound);
                return response;
            }
            int totalVoterInCampaign = 0;
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
            int totalCandidateInCampaign = 0;
            foreach (var i in groupCandidate)
            {
                var ListCandidateGroup = new StatisticalCandidateInGroupResponse();
                ListCandidateGroup.GroupId = i.GroupId;
                ListCandidateGroup.GroupName = i.Name;
                var groupUser = await dbContext.GroupUsers.Where(p => p.CampaignId == campaignId && p.GroupId == i.GroupId).ToListAsync();
                ListCandidateGroup.TotalCandidateInGroup = groupUser.Count();
                listCandidate.Add(ListCandidateGroup);
                totalCandidateInCampaign += ListCandidateGroup.TotalCandidateInGroup;
            }
            result.TotalVoterInCampaign = totalVoterInCampaign;
            result.TotalCandiadteInCampaign = totalCandidateInCampaign;
            result.StatisticalVoterInGroup = listVoter;
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
            var typeNameCheck = await dbContext.Groups.SingleOrDefaultAsync(c => c.Name.ToUpper().Trim().Equals(request.Name.ToUpper().Trim()));
            if (typeNameCheck != null)
            {
                response.ToFailedResponse("Tên nhóm đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }


            groupCheck.Name = request.Name;
            groupCheck.Description = request.Description;
            groupCheck.IsVoter = request.IsVoter;
            dbContext.Groups.Update(groupCheck);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GroupResponse>(groupCheck);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
