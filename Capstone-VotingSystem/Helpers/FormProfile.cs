using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class FormProfile : Profile
    {
        public FormProfile()
        {
            CreateMap<Entities.Form, Models.ResponseModels.FormResponse.GetFormResponse>().ReverseMap();
            CreateMap<Entities.Form, Models.ResponseModels.FormResponse.GetListQuestionFormResponse>().ReverseMap();
        }
    }
}
