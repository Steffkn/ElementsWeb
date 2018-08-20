using AutoMapper;
using Elements.Data;
using Elements.Models.Forum;
using Elements.Services.Admin.Interfaces;

namespace Elements.Services.Admin
{
    public class ManageNewsService : BaseEFService, IManageNewsService
    {
        public ManageNewsService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public bool AddNews(Topic topic)
        {
            this.Context.Topics.Add(topic);
            this.Context.SaveChanges();

            return true;
        }
    }
}
