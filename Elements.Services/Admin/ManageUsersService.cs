namespace Elements.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Elements.Data;
    using Elements.Models;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Elements.Services.Public.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class ManageUsersService : BaseEFService, IManageUsersService
    {
        private readonly IDateTimeService dateTimeService;

        public ManageUsersService(ElementsContext context, IMapper mapper, IDateTimeService dateTimeService)
            : base(context, mapper)
        {
            this.dateTimeService = dateTimeService;
        }

        public IEnumerable<AdministrateUserViewModel> GetAllUsersWithTopics()
        {
            // TODO: add range for pagination
            var users = this.Context.Users.Include(u => u.Topics);
            var result = this.Mapper.Map<IEnumerable<AdministrateUserViewModel>>(users);
            return result;
        }

        public IEnumerable<AdministrateUserViewModel> GetUsers()
        {
            var users = this.Context.Users.ToList();
            var result = this.Mapper.Map<IEnumerable<AdministrateUserViewModel>>(users);
            return result;
        }

        public AdministrateUserViewModel GetUserWithTopics(string userId)
        {
            var user = this.Context.Users.Include(u => u.Topics).FirstOrDefault(u => u.Id == userId);
            AdministrateUserViewModel result = null;

            if (user != null)
            {
                result = this.Mapper.Map<AdministrateUserViewModel>(user);
            }

            return result;
        }

        public UserDetailsViewModel GetUserWithTopicsAndReplies(string userId)
        {
            var user = this.Context.Users
                .Include(u => u.Topics)
                .Include(u => u.Replies)
                .FirstOrDefault(u => u.Id == userId);

            UserDetailsViewModel result = null;

            if (user != null)
            {
                result = this.Mapper.Map<UserDetailsViewModel>(user);
            }

            return result;
        }

        public User RestrictUser(string id)
        {
            var user = this.Context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || user.IsRestricted)
            {
                return null;
            }

            user.IsRestricted = true;
            user.RestrictionEndDate = dateTimeService.Now.AddDays(1);

            this.Context.SaveChanges();

            return user;
        }

        public User RestoreUser(string id)
        {
            var user = this.Context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null || !user.IsRestricted)
            {
                return null;
            }

            user.IsRestricted = false;
            user.RestrictionEndDate = dateTimeService.Now;

            this.Context.SaveChanges();

            return user;
        }
    }
}
