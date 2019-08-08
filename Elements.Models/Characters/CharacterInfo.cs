using System;
using System.ComponentModel.DataAnnotations;

namespace Elements.Models.Characters
{
    public class CharacterInfo
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int CharacterId { get; set; }
        public Character Character { get; set; }

        [Required]
        public double LocationX { get; set; }
        [Required]
        public double LocationY { get; set; }
        [Required]
        public double LocationZ { get; set; }
        [Required]
        public double RotationX { get; set; }
        [Required]
        public double RotationY { get; set; }
        [Required]
        public double RotationZ { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
