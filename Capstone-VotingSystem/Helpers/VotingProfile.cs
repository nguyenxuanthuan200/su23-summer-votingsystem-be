using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class VotingProfile : Profile
    {
        public VotingProfile()
        {
            CreateMap<Entities.Voting, Models.ResponseModels.VotingResponse.CreateVoteResponse>().ReverseMap();
            CreateMap<Entities.Voting, Models.ResponseModels.VotingResponse.UpdateVoteResponse>().ReverseMap();
            CreateMap<Entities.VotingDetail, Models.ResponseModels.VotingDetailResponse.CreateVoteDetailResponse>().ReverseMap();
        }
    }
}
