namespace Elements.Web.Mapping
{
    using AutoMapper;
    using Elements.Models;
    using Elements.Models.Characters;
    using Elements.Models.Forum;
    using Elements.Services.Models.Areas.Admin.BindingModels;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Elements.Services.Models.Character.BindingModel;
    using Elements.Services.Models.Forum.ViewModels;
    using System.Collections.Generic;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<ForumCategory, CategoryViewModel>()
                .ForMember(dest => dest.NumberOfTopics, opt => opt.MapFrom(src => src.Topics.Count));

            this.CreateMap<ForumCategory, SelectCategoryViewModel>();

            this.CreateMap<EditCategoryBindingModel, ForumCategory>()
                .ForMember(dest => dest.CategoryType, opt => opt.MapFrom(src => (ForumCategoryType)src.MainCategoryId))
                .ForMember(dest => dest.Topics, opt => opt.AllowNull());

            this.CreateMap<ForumCategory, EditCategoryViewModel>()
                .ForMember(dest => dest.ImageFile, opt => opt.AllowNull())
                .ForMember(dest => dest.MainCategoryId, opt => opt.MapFrom(src => (int)src.CategoryType))
                .ForMember(dest => dest.MainCategories, opt => opt.UseValue(new HashSet<SelectCategoryViewModel>()));

            this.CreateMap<AddCategoryBindingModel, ForumCategory>();

            this.CreateMap<User, AdministrateUserViewModel>();

            this.CreateMap<User, UserDetailsViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.AllowNull());

            this.CreateMap<User, AuthorViewModel>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));


            this.CreateMap<TopicType, TopicTypeViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => (int)src))
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.ToString()));

            this.CreateMap<AddCharacterBindingModel, Character>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());
        }
    }
}
