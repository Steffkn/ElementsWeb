namespace Elements.Services.Models.Forum.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ReplyViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        [Display(Name = "Date created")]
        public DateTime CreateDate { get; set; }

        public AuthorViewModel AuthorViewModel { get; set; }
    }
}
