using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class CampaignStageProfile : Profile
    {
        public CampaignStageProfile()
        {
            CreateMap<Entities.Stage, Models.ResponseModels.StageResponse.GetStageResponse>().ReverseMap();
            CreateMap<Entities.Stage, Models.ResponseModels.StageResponse.CreateStageResponse>().ReverseMap();
        }
    }
}
