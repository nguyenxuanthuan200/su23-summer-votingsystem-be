using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class CampaignStageProfile : Profile
    {
        public CampaignStageProfile()
        {
            CreateMap<Entities.Stage, Models.ResponseModels.CampaignStageResponse.GetCampaignStageByCampaignResponse>().ReverseMap();
            CreateMap<Entities.Stage, Models.ResponseModels.CampaignStageResponse.CreateCampaginStageResponse>().ReverseMap();
        }
    }
}
