namespace Elements.Services.Models.Forum.BindingModels
{
    using Elements.Services.Models.Forum.ViewModels;
    using System.ComponentModel.DataAnnotations;

    public class ReplyBindingModel
    {
        [Required]
        public string Content { get; set; }

        [Required]
        public AuthorViewModel AuthorViewModel { get; set; }
    }
}
