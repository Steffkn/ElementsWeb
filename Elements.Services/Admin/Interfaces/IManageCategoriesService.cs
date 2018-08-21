namespace Elements.Services.Admin.Interfaces
{
    using Elements.Models.Forum;
    using Elements.Services.Models.Areas.Admin.BindingModels;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using System.Collections.Generic;

    public interface IManageCategoriesService
    {
        IEnumerable<CategoryViewModel> GetCategories();

        CategoryViewModel GetCategoryById(int categoryId);

        bool Add(ForumCategory model);

        bool EditCategory(CategoryBindingModel model);

        bool DeleteCategory(CategoryBindingModel model);
    }
}
