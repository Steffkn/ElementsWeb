using AutoMapper;
using Elements.Common;
using Elements.Data;
using Elements.Models;
using Elements.Services.Public.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Elements.Services.Public
{
    public class UserService : BaseEFService, IUserService
    {
        public UserService(
            ElementsContext context,
            IMapper mapper)
            : base(context, mapper)
        {
        }

        public async Task SetAvatarAsync(string id, string avatarUrl)
        {
            var user = this.Context.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(avatarUrl))
                {
                    avatarUrl = Constants.DefaultAvatar;
                }

                user.Avatar = avatarUrl;
                await this.Context.SaveChangesAsync();
            }
        }

        public User GetUserById(string id)
        {
            var user = this.Context.Users.FirstOrDefault(u => u.Id == id);

            return user;
        }
    }
}
