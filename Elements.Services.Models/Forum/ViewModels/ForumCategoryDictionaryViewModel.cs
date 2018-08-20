using Elements.Models.Forum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elements.Services.Models.Forum.ViewModels
{
    public class ForumCategoryDictionaryViewModel
    {
        public Dictionary<ForumCategoryType, List<ForumCategoryViewModel>> ForumCategories { get; set; }
    }
}
