using Capstone_VotingSystem.Models.ResponseModels.CampaignResponse;
using Capstone_VotingSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Capstone_VotingSystem.Models.RequestModels.CampaignRequest;
using AutoMapper;

namespace Capstone_VotingSystem.Repositories.CampaignRepo
{
    public class CampaignRepositories : ICampaignRepositories
    {
        private readonly VotingSystemContext dbContext;
        private readonly IMapper _mapper;

        public CampaignRepositories(VotingSystemContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<GetCampaignResponse> CreateCampaign(CreateCampaignRequest request)
        {
            var id = Guid.NewGuid();
            Campaign cam = new Campaign();
            {
                cam.CampaignId = id;
                cam.StartTime = request.StartTime;
                cam.Endtime = request.Endtime;
                cam.Status = request.Status;
                cam.CampaignTypeId = request.CampaignTypeId;
                cam.CampusId = request.CampusId;
            };
            await dbContext.Campaigns.AddAsync(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            return map;
        }

        public async Task<IEnumerable<GetCampaignResponse>> GetCampaign()
        {
            var campaign = await dbContext.Campaigns.ToListAsync();
            IEnumerable<GetCampaignResponse> result = campaign.Select(
                x =>
                {
                    return new GetCampaignResponse()
                    {
                        CampaignId = x.CampaignId,
                        StartTime = x.StartTime,
                        EndTime = x.Endtime,
                        Status = x.Status,
                        CampaignTypeId = x.CampaignTypeId,
                        CampusId = x.CampusId,
                    };
                }
                ).ToList();
            return result;
        }

        public async Task<IEnumerable<GetCampaignResponse>> GetCampaignByCampus(Guid id)
        {
            var listCampaign = await dbContext.Campaigns
                .Where(p => p.CampusId == id).ToListAsync();
            List<GetCampaignResponse> list = new List<GetCampaignResponse>();
            foreach (var x in listCampaign)
            {
                var cam = await GetCampaignById(x.CampaignId);
                if (cam != null)
                {
                    list.Add(cam);
                }
            }
            return list;
        }

        public async Task<GetCampaignResponse> GetCampaignById(Guid id)
        {
            var getById = await dbContext.Campaigns.Where(p => p.CampaignId == id && p.Status == true)
                .SingleOrDefaultAsync();
            if (getById == null)
            {
                return null;
            }
            var map = _mapper.Map<GetCampaignResponse>(getById);
            return map;
        }

        public async Task<IEnumerable<GetCampaignResponse>> GetCampaignByType(Guid id)
        {
            var listCampaign = await dbContext.Campaigns
                .Where(p => p.CampaignTypeId == id).ToListAsync();
            List<GetCampaignResponse> list = new List<GetCampaignResponse>();
            foreach (var x in listCampaign)
            {
                var cam = await GetCampaignById(x.CampaignId);
                if (cam != null)
                {
                    list.Add(cam);
                }
            }
            return list;
        }

        public async Task<GetCampaignResponse> UpdateCampaign(UpdateCampaignRequest request)
        {
            var cam = await dbContext.Campaigns.Where(p => p.Status == true).SingleOrDefaultAsync(c => c.CampaignId == request.CampaignId);
            if (cam == null) return null;
            cam.StartTime = request.StartTime;
            cam.Endtime = request.Endtime;
            cam.Status = request.Status;
            cam.CampaignTypeId = request.CampaignTypeId;
            cam.CampusId = request.CampusId;
            dbContext.Campaigns.Update(cam);
            await dbContext.SaveChangesAsync();
            var map = _mapper.Map<GetCampaignResponse>(cam);
            return map;
        }
    }
}
