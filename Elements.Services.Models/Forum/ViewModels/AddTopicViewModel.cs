namespace Elements.Services.Models.Forum.ViewModels
{
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
    }
}
