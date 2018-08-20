namespace Elements.Services.Models.Forum.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TopicWithRepliesViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Title")]
        public string TopicTitle { get; set; }

        [Display(Name = "Content")]
        public string TopicContent { get; set; }

        public DateTime CreateDate { get; set; }

        public AuthorViewModel TopicAuthorViewModel { get; set; }

        public IEnumerable<ReplyViewModel> Replies { get; set; }
    }
}
