namespace Elements.Services.Models.Forum.BindingModels
{
    using System.ComponentModel.DataAnnotations;

    public class AddTopicBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}
