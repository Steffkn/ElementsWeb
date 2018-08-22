using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elements.Services.Models.Forum.ViewModels
{
    public class AuthorViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string Avatar { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}
