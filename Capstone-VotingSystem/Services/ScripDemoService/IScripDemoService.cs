using Capstone_VotingSystem.Core.CoreModel;

namespace Capstone_VotingSystem.Services.ScripDemoService
{
    public interface IScripDemoService
    {
        Task<APIResponse<string>> ScripVote(Guid CampaignId,Guid StageId);
        Task<APIResponse<string>> ScripUnVote(Guid CampaignId, Guid StageId);
    }
}
