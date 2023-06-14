﻿using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class CandidateProfile : Profile
    {
        public CandidateProfile()
        {
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.GetCandidateCampaignResponse>().ReverseMap();
            CreateMap<Entities.Candidate, Models.ResponseModels.CandidateResponse.UpdateCandidateProfileResponse>().ReverseMap();
            CreateMap<Entities.User, Models.ResponseModels.CandidateResponse.UpdateCandidateProfileResponse>().ReverseMap();
        }
    }
}
