using AutoMapper;
using Capstone_VotingSystem.Entities;
using Capstone_VotingSystem.Models.RequestModels.CampaignStageRequest;
using Capstone_VotingSystem.Models.ResponseModels.CampaignStageResponse;
using Microsoft.EntityFrameworkCore;

namespace Capstone_VotingSystem.Repositories.CampaignStageRepo
{
    public class CampaignStageService : ICampaignStageService
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public CampaignStageService(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
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
                campaignStage.Description = request.Description;
                campaignStage.Title = request.Title;
                campaignStage.Status = request.Status;
                campaignStage.Text = request.Text;
                campaignStage.StartTime = request.StartTime;
                campaignStage.EndTime = request.EndTime;
            };
            await dbContext.CampaignStages.AddAsync(campaignStage);
            await dbContext.SaveChangesAsync();
            var re = _mapper.Map<CreateCampaginStageResponse>(campaignStage);
            return re;
        }

        public async Task<IEnumerable<GetCampaignStageByCampaignResponse>> GetCampaignStageByCampaign(Guid campaignId)
        {
            var campaign = await dbContext.CampaignStages.Where(p => p.CampaignId == campaignId).ToListAsync();
            IEnumerable<GetCampaignStageByCampaignResponse> result = campaign.Select(
                x =>
                {
                    return new GetCampaignStageByCampaignResponse()
                    {
                        CampaignStageId = x.CampaignStageId,
                        CampaignId = x.CampaignId,
                        Description = x.Description,
                        Title = x.Title,
                        StartTime = x.StartTime,
                        EndTime = x.StartTime,
                        Text = x.Text,
                        Status = x.Status,

                    };
                }
                ).ToList();
            return result;
        }

        public async Task<GetCampaignStageByCampaignResponse> UpdateCampaignStage(UpdateCampaignStageRequest request)
        {
            var upCam = await dbContext.CampaignStages.SingleOrDefaultAsync(c => c.CampaignStageId == request.CampaignStageId);
            if (upCam == null) return null;
            var checkcampaign = await dbContext.Campaigns.SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId);
            if (upCam == null) return null;

            upCam.Description = upCam.Description;
            upCam.Title = upCam.Title;
            upCam.StartTime = upCam.StartTime;
            upCam.EndTime = upCam.EndTime;
            upCam.Text = upCam.Text;
            upCam.Status = upCam.Status;
            dbContext.CampaignStages.Update(upCam);
            await dbContext.SaveChangesAsync();
            var result = _mapper.Map<GetCampaignStageByCampaignResponse>(upCam);
            return result;
        }

    }
}
