namespace Elements.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Identity;
    using Elements.Models.Forum;
    using Elements.Models.Characters;

    public class User : IdentityUser
    {
        public User()
        {
            this.Topics = new HashSet<Topic>();
            this.Replies = new HashSet<Reply>();
            this.Characters = new HashSet<Character>();
        }

        public string Avatar { get; set; }

        public ICollection<Topic> Topics { get; set; }

        public ICollection<Reply> Replies { get; set; }

        public ICollection<Character> Characters { get; set; }

        public bool IsRestricted { get; set; }

        public DateTime RestrictionEndDate { get; set; }

        [Required]
        public DateTime RegisterDate { get; set; }
    }
}
