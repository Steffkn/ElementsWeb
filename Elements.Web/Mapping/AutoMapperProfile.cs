using AutoMapper;
using Elements.Models.Forum;
using Elements.Web.Areas.Admin.Models.Category;

namespace Elements.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<ForumCategory, CategoryViewModel>()
                .ForMember(dest => dest.NumberOfTopics, opt => opt.MapFrom(src => src.Topics.Count));
        }
    }
}
