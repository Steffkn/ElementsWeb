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

        T GetCategoryById<T>(int categoryId) where T : class;

        Task<bool> AddAsync(ForumCategory model);

        Task<bool> EditCategoryAsync(EditCategoryBindingModel model);

        Task<bool> DeleteCategoryAsync(int id);
    }
}
