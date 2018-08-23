namespace Elements.Services.Models.Forum.ViewModels
{
    using Elements.Models.Forum;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class TopicOfCategoryViewModel
    {
        public int TopicId { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }


        [Display(Name = "Author name")]
        public string AuthorName { get; set; }

        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Number of reply")]
        public int NumberOfReply { get; set; }

        [Display(Name = "Topic importance")]
        public TopicType TopicType { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
    }
}
