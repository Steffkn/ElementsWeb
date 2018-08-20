using System;
using System.Collections.Generic;
using System.Text;
using Elements.Models.Forum;

namespace Elements.Services.Admin.Interfaces
{
    public interface IManageNewsService
    {
        bool AddNews(Topic topic);
    }
}
