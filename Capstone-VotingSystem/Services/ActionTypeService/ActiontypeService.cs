using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.TypeActionRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActionTypeResponse;
using Microsoft.EntityFrameworkCore;
using Octokit;

namespace Capstone_VotingSystem.Services.ActionTypeService
{
    public class ActiontypeService : IActionTypeService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public ActiontypeService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            this._mapper = mapper;
        }

        public async Task<APIResponse<ActionTypeResponse>> CreateTypeAction(ActionTypeRequest request)
        {
            APIResponse<ActionTypeResponse> response = new();

            var checkcate = await dbContext.TypeActions.Where(x => x.Name.ToUpper().Equals(request.Name.ToUpper())).SingleOrDefaultAsync();
            if (checkcate != null)
            {
                response.ToFailedResponse("Name Type đã tồn tại", StatusCodes.Status400BadRequest);
                return response;
            }
            var id = Guid.NewGuid();
            TypeAction type = new TypeAction();
            {
                type.TypeActionId = id;
                type.Name = request.Name;


            }
            await dbContext.TypeActions.AddAsync(type);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<ActionTypeResponse>(type);
            response.ToSuccessResponse("Tạo Type thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<ActionTypeResponse>>> GetAll()
        {
            APIResponse<IEnumerable<ActionTypeResponse>> response = new();
            var actionType = await dbContext.TypeActions.ToListAsync();
            if (actionType == null)
            {
                response.ToFailedResponse("Không tìm thấy", StatusCodes.Status400BadRequest);
                return response;
            }
            IEnumerable<ActionTypeResponse> result = actionType.Select(x =>
            {
                return new ActionTypeResponse()
                {
                    TypeActionId = x.TypeActionId,
                    Name = x.Name,
                };
            }).ToList();
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách Type thành công", StatusCodes.Status200OK);
            return response;
        }
    }

}
