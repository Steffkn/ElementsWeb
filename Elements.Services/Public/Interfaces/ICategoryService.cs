using System;
using System.Collections.Generic;
using System.Text;
using Elements.Models.Forum;
using Elements.Services.Models.Forum.ViewModels;

namespace Elements.Services.Public.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<ForumCategoryViewModel> GetAllCategories();
        Dictionary<ForumCategoryType, List<ForumCategoryViewModel>> GetAllCategoriesInGroups();
        IEnumerable<SelectCategoryViewModel> GetAllCategoriesForSelect();

    }
}
