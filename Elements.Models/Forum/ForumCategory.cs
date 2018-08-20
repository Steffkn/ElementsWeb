namespace Elements.Models.Forum
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class ForumCategory
    {
        public ForumCategory()
        {
            this.Topics = new HashSet<Topic>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Topic> Topics { get; set; }

        [Required]
        [EnumDataType(typeof(ForumCategoryType))]
        public ForumCategoryType CategoryType { get; set; }

        public string IconUrl { get; set; }
    }
}
