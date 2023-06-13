using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class ElementProfile : Profile
    {
        public ElementProfile()
        {
            CreateMap<Entities.Element, Models.ResponseModels.ElementResponse.GetElementResponse>().ReverseMap();
        }
    }
}
