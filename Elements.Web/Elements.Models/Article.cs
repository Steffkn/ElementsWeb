using System;

namespace Elements.Models
{
    public class Article
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string AuthorName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ArticleImageURL { get; set; }
    }
}
