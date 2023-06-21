using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.Group;
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
        public async Task<APIResponse<GroupResponse>> CreateGroup(CreateGroupRequest request)
        {
            APIResponse<GroupResponse> response = new();

            var checkcate = await dbContext.Groups.Where(x => x.Name.ToUpper().Equals(request.Name.ToUpper())).SingleOrDefaultAsync();
            if (checkcate != null)
            {
                response.ToFailedResponse("Name Group đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            Group type = new Group();
            {
                type.GroupId = id;
                type.Name = request.Name;
                type.Description = request.Description;

            }
            await dbContext.Groups.AddAsync(type);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GroupResponse>(type);
            response.ToSuccessResponse("Tạo Group thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

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
                };
            }).ToList();
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách Group thành công", StatusCodes.Status200OK);
            return response;
        }

        public async Task<APIResponse<GroupResponse>> UpdateGroup(Guid id, UpdateGroupRequest request)
        {
            APIResponse<GroupResponse> response = new();
            var typecheck = await dbContext.Groups.SingleOrDefaultAsync(c => c.GroupId == id);
            if (typecheck == null)
            {
                response.ToFailedResponse("Group không tồn tại hoặc đã bị xóa", StatusCodes.Status400BadRequest);
                return response;
            }
            var typeNameCheck = await dbContext.Groups.SingleOrDefaultAsync(c => c.Name.ToUpper().Trim().Equals(request.Name.ToUpper().Trim()));
            if (typeNameCheck != null)
            {
                response.ToFailedResponse("Group Name đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }


            typecheck.Name = request.Name;
            typecheck.Description = request.Description;
            dbContext.Groups.Update(typecheck);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GroupResponse>(typecheck);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }
    }
}
