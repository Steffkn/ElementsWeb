using System.ComponentModel.DataAnnotations;

namespace Elements.Models.Props.Items
{
    public abstract class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
