using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.CampaignRepo
{
    public class CampaignRepositories : ICampaignRepositories
    {
        private readonly VotingSystemContext dbContext;

        public CampaignRepositories(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }
    
        public async Task<IEnumerable<GetCampaignResponse>> GetCampaign()
        {
            var campaign = await dbContext.Campaigns.ToListAsync();
            IEnumerable<GetCampaignResponse> result = campaign.Select(
                x =>
                {
                    return new GetCampaignResponse()
                    {
                        Id = x.CampaignId,
                        TimeStart = x.StartTime,
                        TimeEnd = x.Endtime,
                        Status = x.Status,
                        CampaignTypeId = x.CampaignTypeId,
                        CampusId = x.CampusId,
                    };
                }
                ).ToList();
            return result;
        }
    }
}
