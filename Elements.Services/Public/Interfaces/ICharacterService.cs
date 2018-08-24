namespace Elements.Services.Public.Interfaces
{
    using Elements.Models;
    using Elements.Models.Characters;
    using Elements.Services.Models.Character.BindingModel;
    using Elements.Services.Models.Character.ViewModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICharacterService
    {
        Task<bool> AddCharacter(AddCharacterBindingModel model, string userId);

        IEnumerable<Character> GetCharactersForUser(string id);
    }
}
