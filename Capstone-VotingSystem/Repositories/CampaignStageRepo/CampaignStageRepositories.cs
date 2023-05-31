using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.CampaignStageRepo
{
    public class CampaignStageRepositories : ICampaignStageRepositories
    {
        private readonly VotingSystemContext dbContext;

        public CampaignStageRepositories(VotingSystemContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<CreateCampaginStageResponse> CreateCampaignStage(CreateCampaignStageRequest request)
        {
            var campaign = await dbContext.Campaigns.Where(
             p => p.CampaignId == request.CampaignId).SingleOrDefaultAsync();

            if (campaign == null)
            {
                return null;
            }
            var id = Guid.NewGuid();
            CampaignStage campaignStage = new CampaignStage();
            {
                campaignStage.CampaignStageId = id;
                campaignStage.CampaignId = request.CampaignId;
            };
            await dbContext.CampaignStages.AddAsync(campaignStage);
            await dbContext.SaveChangesAsync();
            //var re = _mapper.map<createpostresponse>(post);
            //var mapproduct = _mapper.map<getproductresponse>(product);
            //re.product = mapproduct;
            CreateCampaginStageResponse cam = new CreateCampaginStageResponse();
            {
                cam.CampaignStageId = campaignStage.CampaignStageId;
                cam.CampaignId = campaignStage.CampaignId;
            }
            return cam ;
        }

        public async Task<IEnumerable<GetCampaignStageByCampaignResponse>> GetCampaignStageByCampaign(Guid campaignId)
        {
            var campaign = await dbContext.CampaignStages.Where(p=>p.CampaignId== campaignId).ToListAsync();
            IEnumerable<GetCampaignStageByCampaignResponse> result = campaign.Select(
                x =>
                {
                    return new GetCampaignStageByCampaignResponse()
                    {
                        CampaignStageId = x.CampaignStageId,
                        CampaignId = x.CampaignId,
                        AmountVote = x.AmountVote,
                    };
                }
                ).ToList();
            return result;
        }

        public async Task<GetCampaignStageByCampaignResponse> UpdateCampaignStageVote(Guid id)
        {
            var upCam = await dbContext.CampaignStages.SingleOrDefaultAsync(c => c.CampaignStageId == id);
            if (upCam == null) return null;

            upCam.AmountVote = upCam.AmountVote;
            dbContext.CampaignStages.Update(upCam);
            await dbContext.SaveChangesAsync();
            GetCampaignStageByCampaignResponse up = new GetCampaignStageByCampaignResponse();
            {
                up.CampaignStageId = upCam.CampaignStageId;
                up.CampaignId = upCam.CampaignId;
                up.AmountVote = upCam.AmountVote;
            }
            return up;
        }

    }
}
