namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Models.Characters;
    using Elements.Services.Public.Interfaces;
    using Elements.Web.API.Models;
    using Elements.Web.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    /// <summary>
    /// 
    /// api/users/{userId}
    /// api/users/{userId}/characters
    /// </summary>
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly ICharacterService characterService;
        private readonly AutoMapper.IMapper mapper;

        public UsersController(IUserService userService, ICharacterService characterService, AutoMapper.IMapper mapper)
        {
            this.userService = userService;
            this.characterService = characterService;
            this.mapper = mapper;
        }

        // GET api/users/{userId}
        [HttpGet("{userId}")]
        public ActionResult Get(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                Models.User user = this.userService.GetUserById(userId);

                API.Models.BaseUser apiBaseUser = this.mapper.Map<Models.User, API.Models.BaseUser>(user);
                return this.Json(apiBaseUser);
            }

            // TODO: 3 extract these for all requests
            return null;
        }

        // GET api/users/{userId}/characters
        [HttpGet("{userId}/characters")]
        public ActionResult GetCharacters(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                IEnumerable<Character> character = this.characterService.GetCharactersForUser(userId);
                IEnumerable<BaseCharacter> baseCharacter = this.mapper.Map<IEnumerable<Character>, IEnumerable<BaseCharacter>>(character);
                return this.Json(baseCharacter);
            }

            // TODO: 4 extract these for all requests
            return null;
        }
    }
}