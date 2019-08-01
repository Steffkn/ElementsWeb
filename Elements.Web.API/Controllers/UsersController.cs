﻿namespace Elements.Web.Areas.Admin.Controllers
{
    using Elements.Models;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Public.Interfaces;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// api/users/{userId}
    /// api/users/{userId}/characters
    /// </summary>
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly ICharacterService characterService;

        public UsersController(IUserService userService, ICharacterService characterService)
        {
            this.userService = userService;
            this.characterService = characterService;
        }

        // GET api/users/{userId}
        [HttpGet("{userId}")]
        public ActionResult Get(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return this.Json(this.userService.GetUserById(userId));
            }

            // TODO: 3 extract these for all requests
            return null;
        }

        // GET api/users/{userId}
        [HttpGet("{userId}/characters")]
        public ActionResult GetCharacters(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                return this.Json(this.characterService.GetCharactersForUser(userId));
            }

            // TODO: 4 extract these for all requests
            return null;
        }
    }
}