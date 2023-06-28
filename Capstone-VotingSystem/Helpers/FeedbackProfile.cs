using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Entities.FeedBack, Models.ResponseModels.FeedbackResponse.FeedbackResponse>().ReverseMap();
        }
    }
}
