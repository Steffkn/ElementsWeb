using AutoMapper;
using Elements.Models.Characters;
using Elements.Services.Models.Character.BindingModel;
using Elements.Services.Models.Character.ViewModel;
using Elements.Services.Public.Interfaces;
using Elements.Web.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elements.Web.Controllers
{
    [Authorize]
    public class CharacterController : Controller
    {
        private readonly ICharacterService characterService;
        private readonly IMapper mapper;

        public CharacterController(ICharacterService characterService, IMapper mapper)
        {
            this.characterService = characterService;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult All()
        {
            var characters = this.characterService.GetCharactersForUser(this.User.GetUserId());
            var charactersViewModel = this.mapper.Map<IEnumerable<Character>, IEnumerable<SimpleCharacterViewModel>>(characters);
            return this.View(charactersViewModel);
        }

        [HttpGet]
        public IActionResult AddCharacter()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCharacter(AddCharacterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                AddCharacterViewModel viewModel = this.mapper.Map<AddCharacterBindingModel, AddCharacterViewModel>(model);
                return this.View(viewModel);
            }

            bool result = await this.characterService.AddCharacter(model, this.User.GetUserId());

            if (!result)
            {
                this.ModelState.AddModelError(string.Empty, "Character name is already taken!");
                return this.View();
            }

            return this.RedirectToAction("Index", "Forum", null);
        }
    }
}
