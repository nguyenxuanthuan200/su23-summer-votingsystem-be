using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class CampaignStageProfile : Profile
    {
        public CampaignStageProfile()
        {
            CreateMap<Entities.CampaignStage, Models.ResponseModels.CampaignStageResponse.GetCampaignStageByCampaignResponse>().ReverseMap();
            CreateMap<Entities.CampaignStage, Models.ResponseModels.CampaignStageResponse.CreateCampaginStageResponse>().ReverseMap();
        }
    }
}
