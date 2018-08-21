namespace Elements.Services.Models.Areas.Admin.ViewModels
{
    using Elements.Models.Forum;
    using Elements.Services.Models.Forum.ViewModels;
    using Microsoft.AspNetCore.Http;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AddCategoryViewModel
    {
        public AddCategoryViewModel()
        {
            this.MainCategories = new HashSet<SelectCategoryViewModel>();
        }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public IFormFile ImageFile { get; set; }

        [Required]
        public int MainCategoryId { get; set; }

        public IEnumerable<SelectCategoryViewModel> MainCategories { get; set; }
    }
}
