using AutoMapper;
using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.ActionHistoryRequest;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;
using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;


namespace Capstone_VotingSystem.Services.ActionHistoryService
{
    public class ActionHistoryService : IActionHistoryService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public ActionHistoryService(VotingSystemContext votingSystemContext, IMapper mapper)
        {
            this.dbContext = votingSystemContext;
            this._mapper = mapper;
        }

        public async Task<APIResponse<CreateActionHistoryResponse>> CreateActionHistory(ActionHistoryRequest request)
        {
            APIResponse<CreateActionHistoryResponse> response = new();
            var checkUser = await dbContext.Users.SingleOrDefaultAsync(p => p.UserId == request.UserId);
            if (checkUser == null)
            {
                response.ToFailedResponse("không tìm thấy user", StatusCodes.Status404NotFound);
                return response;
            }
            var id = Guid.NewGuid();
            HistoryAction actionHistory = new HistoryAction();
            {
                actionHistory.HistoryActionId = id;
                actionHistory.UserId = request.UserId;
                actionHistory.Time = DateTime.UtcNow;
                actionHistory.TypeActionId = request.TypeActionId;
                actionHistory.Description = request.Description;
            }
            await dbContext.HistoryActions.AddAsync(actionHistory);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<CreateActionHistoryResponse>(actionHistory);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<UpdateActionHistoryResponse>> UpdateActionHistory(UpdateActionHistoryRequest request, Guid? id)
        {
            APIResponse<UpdateActionHistoryResponse> response = new();
            var checkAction = await dbContext.HistoryActions.SingleOrDefaultAsync(p => p.HistoryActionId == id);
            if (checkAction == null)
            {
                response.ToFailedResponse("không tìm thấy action History", StatusCodes.Status404NotFound);
                return response;
            }
            checkAction.Description = request.Description;
            checkAction.TypeActionId = request.TypeActionId;
            checkAction.Time = DateTime.UtcNow;
            dbContext.HistoryActions.Update(checkAction);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<UpdateActionHistoryResponse>(checkAction);
            response.ToSuccessResponse("Cập nhật thành công", StatusCodes.Status200OK);
            response.Data = map;
            return response;
        }

        public async Task<APIResponse<IEnumerable<ActionHistoryResponse>>> GetActionHistoryByUser(string? userId)
        {
            APIResponse<IEnumerable<ActionHistoryResponse>> response = new();
            var checkUserId = await dbContext.Users.Where(x => x.UserId.Equals(userId)).SingleOrDefaultAsync();
            if (checkUserId == null)
            {
                response.ToFailedResponse("không tồn tại user", StatusCodes.Status404NotFound);
                return response;
            }
            var historyList = await dbContext.HistoryActions.Where(p => p.UserId == userId).ToListAsync();
            List<ActionHistoryResponse> result = new List<ActionHistoryResponse>();
            foreach (var item in historyList)
            {
                var checkTypeAction = await dbContext.TypeActions.SingleOrDefaultAsync(p => p.TypeActionId == item.TypeActionId);
                var actions = new ActionHistoryResponse();
                {
                    actions.HistoryActionId = item.HistoryActionId;
                    actions.Description = item.Description;
                    actions.UserId = item.UserId;
                    actions.TypeActionId = item.TypeActionId;
                    actions.ActionTypeName = checkTypeAction.Name;
                }
                result.Add(actions);
            }
            if (result.Count ==0)
            {
                response.ToFailedResponse("Không có History nào của User", StatusCodes.Status400BadRequest);
                return response;
            }
            response.ToSuccessResponse(response.Data = result, "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
