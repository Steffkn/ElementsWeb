namespace Elements.Services.Models.Forum.ViewModels
{
    using Elements.Models.Forum;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddTopicViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<SelectCategoryViewModel> Categories { get; set; }

        [EnumDataType(typeof(TopicType))]
        public TopicType TopicType { get; set; }

        public IEnumerable<TopicTypeViewModel> TopicTypes { get; set; }

        public IFormFile ImageUrl { get; set; }
    }
}
