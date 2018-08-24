using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Elements.Models;
using Elements.Models.Characters;
using Elements.Services.Models.Character.ViewModel;
using Elements.Services.Models.Forum.ViewModels;
using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Pages
{
    public class UserModel : PageModel
    {
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly ICharacterService characterService;

        public UserModel(UserManager<User> userManager, IMapper mapper, ICharacterService characterService)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.characterService = characterService;
        }

        public AuthorViewModel UserViewModel { get; set; }

        public IEnumerable<SimpleCharacterViewModel> CharactersViewModel { get; set; }

        public async Task OnGet(string id)
        {
            var user = await userManager.FindByNameAsync(id);
            if (user != null)
            {
                var character = characterService.GetCharactersForUser(user.Id);

                this.UserViewModel = this.mapper.Map<User, AuthorViewModel>(user);
                this.UserViewModel.Roles = await this.userManager.GetRolesAsync(user);
                this.CharactersViewModel = this.mapper.Map<IEnumerable<Character>, IEnumerable<SimpleCharacterViewModel>>(character);
            }
        }
    }
}