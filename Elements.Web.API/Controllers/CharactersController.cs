using AutoMapper;
using Elements.Models.Characters;
using Elements.Services.Models.Character.BindingModel;
using Elements.Services.Models.Character.ViewModel;
using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elements.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : Controller
    {
        private readonly ICharacterService characterService;

        public CharactersController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/characters/d64f3da2-a44a-44d0-b71e-9fa6216462c1
        [HttpGet("{userId}")]
        public IEnumerable<Character> Get(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return this.characterService.GetCharactersForUser(userId);
            }
            return null;
        }
    }
}
