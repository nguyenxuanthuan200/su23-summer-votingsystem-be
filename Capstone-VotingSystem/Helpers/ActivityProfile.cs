using AutoMapper;

namespace Capstone_VotingSystem.Helpers
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Entities.Activity, Models.ResponseModels.ActivityResponse.GetActivityResponse>().ReverseMap();
        }
    }
}
