namespace Elements.Services.Models.Forum.ViewModels
{
    using System;

    public class ReplyViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }

        public AuthorViewModel AuthorViewModel { get; set; }
    }
}
