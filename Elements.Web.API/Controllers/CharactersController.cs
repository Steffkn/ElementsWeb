using AutoMapper;
using Elements.Models.Characters;
using Elements.Services.Public.Interfaces;
using Elements.Web.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elements.Web.Controllers
{
    /*
     TODO:
     api/characters/{characterId}
     api/users/{userId}
     api/users/{userId}/characters
         */
    public class CharactersController : BaseController
    {
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public CharactersController(ICharacterService characterService, IMapper mapper)
        {
            this.characterService = characterService;
            this.mapper = mapper;
        }

        // GET api/character/{characterId}
        [HttpGet("{characterId}")]
        public ActionResult GetCharacter(int? characterId)
        {
            if (characterId.HasValue)
            {
                Character character = this.characterService.GetCharacterByID(characterId.Value);

                BaseCharacter baseCharacter = this.mapper.Map<Character, BaseCharacter>(character);
                return this.Json(baseCharacter);
            }

            // TODO: 2 extract these for all requests
            return null;
        }
    }
}
