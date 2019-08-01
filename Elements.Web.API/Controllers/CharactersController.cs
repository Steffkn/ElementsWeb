using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Elements.Web.Controllers
{
    /*
     TODO:
     api/characters/{characterId}
     api/users/{userId}
     api/users/{userId}/characters
         */
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CharactersController : Controller
    {
        private readonly ICharacterService characterService;

        public CharactersController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }


        // GET api/character/{characterId}
        [HttpGet("{characterId}")]
        public ActionResult GetCharacter(int? characterId)
        {
            if (characterId.HasValue)
            {
                return this.Json(this.characterService.GetCharacterByID(characterId.Value));
            }

            // TODO: 2 extract these for all requests
            return null;
        }
    }
}
