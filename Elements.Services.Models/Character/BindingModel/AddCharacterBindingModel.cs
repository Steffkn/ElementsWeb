using Elements.Models.Characters;
using System.ComponentModel.DataAnnotations;

namespace Elements.Services.Models.Character.BindingModel
{
    public class AddCharacterBindingModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        public CharacterClassType CharacterClass { get; set; }

        [Required]
        public BodyType BodyType { get; set; }
    }
}
