using Elements.Models.Props.Inventories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elements.Models.Characters
{
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public CharacterClassType CharacterClass { get; set; }

        public InventorySize InventorySize { get; set; }

        public BodyType BodyType { get; set; }

        public ICollection<CharacterInventory> Inventory { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
