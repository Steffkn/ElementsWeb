namespace Elements.Services.Models.Forum.BindingModels
{
    using Elements.Models.Forum;
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;

    public class AddTopicBindingModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public TopicType TopicType { get; set; }

        public IFormFile ImageUrl { get; set; }
    }
}
