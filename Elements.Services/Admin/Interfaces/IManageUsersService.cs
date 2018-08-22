namespace Elements.Services.Admin.Interfaces
{
    using Elements.Models;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using System.Collections.Generic;

    public interface IManageUsersService
    {
        IEnumerable<AdministrateUserViewModel> GetAllUsersWithTopics();

        IEnumerable<AdministrateUserViewModel> GetUsers();

        AdministrateUserViewModel GetUserWithTopics(string userId);

        UserDetailsViewModel GetUserWithTopicsAndReplies(string userId);

        User RestrictUser(string id);
        User RestoreUser(string id);
    }
}
