namespace Elements.Services.Public.Interfaces
{
    using Elements.Models;
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task SetAvatarAsync(string id, string avatarUrl);
        User GetUserById(string id);
    }
}
