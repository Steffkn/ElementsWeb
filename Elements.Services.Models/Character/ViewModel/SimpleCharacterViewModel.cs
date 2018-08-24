using Elements.Models.Characters;
using System.ComponentModel.DataAnnotations;

namespace Elements.Services.Models.Character.ViewModel
{
    public class SimpleCharacterViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Class")]
        public CharacterClassType CharacterClass { get; set; }
    }
}
