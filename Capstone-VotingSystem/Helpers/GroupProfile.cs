using AutoMapper;
using Capstone_VotingSystem.Entities;

namespace Capstone_VotingSystem.Helpers
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<Group, Models.ResponseModels.GroupResponse.GroupResponse>().ReverseMap();
        }
    }
}
