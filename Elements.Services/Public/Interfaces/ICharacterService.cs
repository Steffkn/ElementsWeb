namespace Elements.Services.Public.Interfaces
{
    using Elements.Models.Characters;
    using Elements.Services.Models.Character.BindingModel;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ICharacterService
    {
        Task<bool> AddCharacter(AddCharacterBindingModel model, string userId);

        IEnumerable<Character> GetCharactersForUser(string id);

        Character GetCharacterByID(int characterId);
    }
}
