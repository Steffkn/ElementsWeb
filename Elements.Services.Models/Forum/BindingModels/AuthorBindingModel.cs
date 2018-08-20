using System.ComponentModel.DataAnnotations;

namespace Elements.Services.Models.Forum.BindingModels
{
    public class AuthorBindingModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
