using AutoMapper;

namespace Elements.Web.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Elements.Models.Characters.Character, API.Models.BaseCharacter>();
            this.CreateMap<Elements.Models.User, API.Models.BaseUser>();
        }
    }
}
