using AutoMapper;

namespace Capstone_VotingSystem.Helpers
{
    public class ActionHistoryProfile : Profile
    {
        public ActionHistoryProfile()
        {
            CreateMap<Entities.HistoryAction, Models.ResponseModels.ActionHistoryResponse.ActionHistoryResponse>().ReverseMap();
            CreateMap<Entities.HistoryAction, Models.ResponseModels.ActionHistoryResponse.CreateActionHistoryResponse>().ReverseMap();
            CreateMap<Entities.HistoryAction, Models.ResponseModels.ActionHistoryResponse.UpdateActionHistoryResponse>().ReverseMap();
        }
    }
}
