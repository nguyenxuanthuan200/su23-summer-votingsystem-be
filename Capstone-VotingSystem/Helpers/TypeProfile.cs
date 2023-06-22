using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class TypeProfile : Profile
    {
        public TypeProfile()
        {
            CreateMap<Entities.Type, Models.ResponseModels.TypeResponse.TypeResponse>().ReverseMap();
        }
    }
}
