using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Elements.Data;
using Elements.Models.Characters;
using Elements.Services.Models.Character.BindingModel;
using Elements.Services.Public.Interfaces;

namespace Elements.Services.Public
{
    public class CharacterService : BaseEFService, ICharacterService
    {
        public CharacterService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<bool> AddCharacter(AddCharacterBindingModel model, string userId)
        {
            if (model == null)
            {
                return false;
            }

            bool characterExists = this.Context.Characters.Any(ch => ch.Name == model.Name);

            if (characterExists)
            {
                return false;
            }

            var entityModel = this.Mapper.Map<Character>(model);
            entityModel.UserId = userId;

            await this.Context.AddAsync(entityModel);
            await this.Context.SaveChangesAsync();
            return true;
        }

        public IEnumerable<Character> GetCharactersForUser(string userId)
        {
            var characters = this.Context.Characters.Where(x => x.UserId == userId);
            return characters;
        }

        public Character GetCharacterByID(int characterId)
        {
            return this.Context.Characters.FirstOrDefault(x => x.Id == characterId);
        }
    }
}
