using System.ComponentModel.DataAnnotations;

namespace Elements.Services.Models.Forum.BindingModels
{
    public class AddReplyBindingModel
    {
        [Required]
        public int TopicId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
