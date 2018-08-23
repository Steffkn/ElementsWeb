using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Admin.Interfaces;
using System.Threading.Tasks;

namespace Elements.Services.Admin
{
    public class ManageNewsService : BaseEFService, IManageNewsService
    {
        public ManageNewsService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task<bool> AddNewsAsync(Topic topic)
        {
            if (topic == null)
            {
                return false;
            }

            await this.Context.Topics.AddAsync(topic);
            await this.Context.SaveChangesAsync();

            return true;
        }
    }
}
