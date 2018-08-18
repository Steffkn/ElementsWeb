namespace Elements.Models.Forum
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Topic
    {
        public Topic()
        {
            this.Replies = new HashSet<Reply>();
            this.IsActive = true;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

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
        public TopicType TopicType { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public ForumCategory Category { get; set; }

        public ICollection<Reply> Replies { get; set; }

        // TODO:  [Url]
        public string ImageUrl { get; set; }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
    }
}
