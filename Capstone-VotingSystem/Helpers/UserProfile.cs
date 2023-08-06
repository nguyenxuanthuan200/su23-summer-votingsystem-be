using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Models.ResponseModels.CandidateResponse.CreateAccountCandidateResponse>().ReverseMap();
            CreateMap<Entities.User, Models.ResponseModels.CandidateResponse.GetCandidateDetailResponse>().ReverseMap();
            CreateMap<Entities.User, Models.ResponseModels.CandidateResponse.UpdateCandidateProfileResponse>().ReverseMap();
            CreateMap<Entities.User, Models.ResponseModels.UserResponse.UpdateUserResponse>().ReverseMap();
            CreateMap<Entities.User, Models.ResponseModels.CandidateResponse.GetListCandidateCampaignResponse>().ReverseMap();
            CreateMap<Entities.User, Models.ResponseModels.UserResponse.GetUserByIdResponse>().ReverseMap();

        }
    }
}
