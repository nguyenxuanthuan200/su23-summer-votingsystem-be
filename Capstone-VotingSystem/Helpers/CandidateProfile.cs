using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            //CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.GetCandidateCampaignResponse>().ReverseMap();
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.UpdateCandidateProfileResponse>().ReverseMap();
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.GetListCandidateCampaignResponse>().ReverseMap();
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.CreateAccountCandidateResponse>().ReverseMap();
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.CreateCandidateCampaignResponse>().ReverseMap();
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.GetCandidateDetailResponse>().ReverseMap();
            
        }
    }
}
