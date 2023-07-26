using AutoMapper;

namespace Capstone_VotingSystem.Helpers
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Entities.Notification, Models.ResponseModels.NotificationResponse.NotificationResponse>().ReverseMap();
        }
    }
}
