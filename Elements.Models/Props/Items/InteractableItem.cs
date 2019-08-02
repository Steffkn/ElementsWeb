using Elements.Models.Props.Inventories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elements.Models.Props.Items
{
    public class InteractableItem : Item
    {
        [Required]
        public bool IsStackable { get; set; }

        [Required]
        public int MaxStackSize { get; set; }

        [Required]
        public bool IsUsable { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public ItemRarity Rarity { get; set; }

        [Required]
        public EquipmentSlot EquipmentSlot { get; set; }

        public int Price { get; set; }

        public ICollection<CharacterInventory> Characters { get; set; }
    }
}
