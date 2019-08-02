using Elements.Models.Characters;
using Elements.Models.Props.Items;
using System.ComponentModel.DataAnnotations;

namespace Elements.Models.Props.Inventories
{
    public class CharacterInventory
    {
        [Required]
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        [Required]
        public int Slot { get; set; }

        [Required]
        public int ItemId { get; set; }
        public InteractableItem Item { get; set; }
    }
}
