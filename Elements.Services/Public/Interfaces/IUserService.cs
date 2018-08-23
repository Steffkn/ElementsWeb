namespace Elements.Services.Public.Interfaces
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        Task SetAvatarAsync(string id, string avatarUrl);
    }
}
