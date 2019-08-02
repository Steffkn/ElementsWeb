using Elements.Models.Characters;
using System.ComponentModel.DataAnnotations;

namespace Elements.Web.API.Models
{
    public class BaseCharacter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public CharacterClassType CharacterClass { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
