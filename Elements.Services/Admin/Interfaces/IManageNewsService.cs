using System.Threading.Tasks;
using Elements.Models.Forum;

namespace Elements.Services.Admin.Interfaces
{
    public interface IManageNewsService
    {
        Task<bool> AddNewsAsync(Topic topic);
    }
}
