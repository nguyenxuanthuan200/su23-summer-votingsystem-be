using AutoMapper;

namespace Capstone_VotingSystem.Helpers
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Entities.Category, Models.ResponseModels.CategoryResponse.GetCategoryResponse>().ReverseMap();
        }
    }
}
