namespace Elements.Services.Models.Forum.ViewModels
{
    using Elements.Models.Forum;
    using System;

    public class TopicOfCategoryViewModel
    {
        public int TopicId { get; set; }

        public string Title { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreateDate { get; set; }

        public int NumberOfReply { get; set; }

        public TopicType TopicType { get; set; }

        public string ImageUrl { get; set; }
    }
}
