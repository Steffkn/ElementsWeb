namespace Elements.Services.Admin.Interfaces
{
    using Elements.Models.Forum;
    using Elements.Services.Models.Areas.Admin.BindingModels;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IManageCategoriesService
    {
        IEnumerable<CategoryViewModel> GetCategories();

        CategoryViewModel GetCategoryById(int categoryId);

        T GetCategoryById<T>(int categoryId) where T : class;

        bool Add(ForumCategory model);

        Task<bool> EditCategoryAsync(EditCategoryBindingModel model);

        bool DeleteCategory(CategoryBindingModel model);
    }
}
