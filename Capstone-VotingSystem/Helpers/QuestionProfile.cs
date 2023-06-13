using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class QuestionProfile : Profile
    {
        public QuestionProfile()
        {
            CreateMap<Entities.Question, Models.ResponseModels.QuestionResponse.GetQuestionResponse>().ReverseMap();
        }
    }
}
