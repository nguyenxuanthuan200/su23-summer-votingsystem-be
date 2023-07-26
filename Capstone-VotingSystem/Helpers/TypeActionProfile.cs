using AutoMapper;

namespace Capstone_VotingSystem.Helpers
{
    public class TypeActionProfile : Profile
    {
        public TypeActionProfile()
        {
            CreateMap<Entities.TypeAction, Models.ResponseModels.ActionTypeResponse.ActionTypeResponse>().ReverseMap();
        }
    }
}
