using AutoMapper;
using Elements.Models;
using Elements.Models.Forum;
using Elements.Services.Models.Areas.Admin.ViewModels;

namespace Elements.Web.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<ForumCategory, CategoryViewModel>()
                .ForMember(dest => dest.NumberOfTopics, opt => opt.MapFrom(src => src.Topics.Count));
            this.CreateMap<User, AdministrateUserViewModel>();

            this.CreateMap<TopicType, TopicTypeViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ToString()));
        }
    }
}
