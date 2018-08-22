using AutoMapper;
using Elements.Data;
using Elements.Services.Public.Interfaces;
using System.Linq;

namespace Elements.Services.Public
{
    public class UserService : BaseEFService, IUserService
    {
        public UserService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public void SetAvatar(string id, string avatarUrl)
        {
            var user = this.Context.Users.FirstOrDefault(u => u.Id == id);
            user.Avatar = avatarUrl;

            this.Context.SaveChanges();
        }
    }
}
