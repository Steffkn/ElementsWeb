using Elements.Models.Characters;
using System.ComponentModel.DataAnnotations;

namespace Elements.Services.Models.Character.ViewModel
{
    public class AddCharacterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Character type")]
        public CharacterClassType CharacterClass { get; set; }
    }
}
