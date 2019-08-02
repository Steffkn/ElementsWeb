using AutoMapper;
using Elements.Models.Characters;
using Elements.Web.API.Models;

namespace Elements.Web.API.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Character, BaseCharacter>();
        }
    }
}
