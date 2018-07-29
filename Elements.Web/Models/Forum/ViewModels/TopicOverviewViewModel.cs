namespace Elements.Web.Models.Forum.ViewModels
{
    using System;

    public class TopicOverviewViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}
