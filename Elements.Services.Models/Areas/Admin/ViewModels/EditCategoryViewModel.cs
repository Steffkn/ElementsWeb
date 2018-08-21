namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    using Elements.Services.Models.Forum.ViewModels;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class EditCategoryViewModel
    {
        public EditCategoryViewModel()
        {
            this.MainCategories = new HashSet<SelectCategoryViewModel>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string IconUrl { get; set; }

        [Display(Name = "Category icon")]
        public IFormFile ImageFile { get; set; }

        [Required]
        [Display(Name = "Main category")]
        public int MainCategoryId { get; set; }

        [Required]
        public bool IsPrivate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public IEnumerable<SelectCategoryViewModel> MainCategories { get; set; }
    }
}
