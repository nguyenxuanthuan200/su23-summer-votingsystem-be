﻿using AutoMapper;
namespace Capstone_VotingSystem.Helpers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Entities.User, Models.ResponseModels.CandidateResponse.CreateAccountCandidateResponse>().ReverseMap();
        }
    }
}
