using Capstone_VotingSystem.Core.CoreModel;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.ResponseModels.ActionHistoryResponse;
using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Models.ResponseModels.CandidateResponse;
using Capstone_VotingSystem.Models.ResponseModels.ElementResponse;
using Capstone_VotingSystem.Models.ResponseModels.QuestionResponse;
using Microsoft.Data.SqlClient.Server;
using Microsoft.EntityFrameworkCore;
using Octokit;
using System.Linq;
using System.Xml.Linq;


namespace Capstone_VotingSystem.Services.ActionHistoryService
{
    public class ActionHistoryService : IActionHistoryService
    {
        private readonly VotingSystemContext dbContext;

        public ActionHistoryService(VotingSystemContext votingSystemContext)
        {
            this.dbContext = votingSystemContext;
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
                    actions.Time=item.Time;
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
            response.ToSuccessResponse(response.Data = result.OrderByDescending(hh => hh.Time), "Lấy danh sách thành công", StatusCodes.Status200OK);
            return response;
        }
    }
}
