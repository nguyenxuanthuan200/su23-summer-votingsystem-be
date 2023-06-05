using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class CampaignProfile : Profile
    {
        public CampaignProfile()
        {
            CreateMap<Entities.Campaign, Models.ResponseModels.CampaignResponse.GetCampaignResponse>().ReverseMap();
        }
    }
}
