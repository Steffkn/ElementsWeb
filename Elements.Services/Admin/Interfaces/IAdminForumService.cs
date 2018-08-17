namespace Elements.Services.Admin.Interfaces
{
    using Elements.Services.Models.Areas.Admin.BindingModels;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using System.Collections.Generic;

    public interface IAdminForumService
    {
        IEnumerable<CategoryViewModel> GetCategories();

        CategoryViewModel GetCategoryById(int categoryId);

        bool EditCategory(CategoryBindingModel model);

        bool DeleteCategory(CategoryBindingModel model);
    }
}
