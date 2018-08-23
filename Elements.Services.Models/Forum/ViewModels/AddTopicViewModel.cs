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
        [Display(Name = "Sub-Category")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectCategoryViewModel> Categories { get; set; }

        [EnumDataType(typeof(TopicType))]
        [Display(Name = "Topic importance")]
        public TopicType TopicType { get; set; }

        public IEnumerable<TopicTypeViewModel> TopicTypes { get; set; }

        [Display(Name = "Icon")]
        public IFormFile ImageUrl { get; set; }
    }
}
