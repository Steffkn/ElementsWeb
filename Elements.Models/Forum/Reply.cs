namespace Elements.Models.Forum
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Reply
    {
        public Reply()
        {
            this.IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public string AuthorId { get; set; }

        public User Author { get; set; }

        [Required]
        public int TopicId { get; set; }

        public Topic Topic { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
