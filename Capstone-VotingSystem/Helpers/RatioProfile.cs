using AutoMapper;

namespace Capstone_VotingSystem.Helpers
{
    public class RatioProfile : Profile
    {
        public RatioProfile()
        {
            CreateMap<Entities.Ratio, Models.ResponseModels.RatioResponse.RatioResponse>().ReverseMap();
        }
    }
}
