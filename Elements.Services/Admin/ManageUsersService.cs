﻿namespace Elements.Services.Admin
{
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using Elements.Data;
    using Elements.Services.Admin.Interfaces;
    using Elements.Services.Models.Areas.Admin.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class ManageUsersService : BaseEFService, IManageUsersService
    {
        public ManageUsersService(ElementsContext context, IMapper mapper)
            : base(context, mapper)
        {
        }

        public IEnumerable<AdministrateUserViewModel> GetAllUsersWithTopics()
        {
            // TODO: add range for pagination
            var usersWithTopics = this.Context.Users.Include(u => u.Topics);
            var result = this.Mapper.Map<IEnumerable<AdministrateUserViewModel>>(usersWithTopics);
            return result;
        }

        public IEnumerable<AdministrateUserViewModel> GetUsers()
        {
            var usersWithTopics = this.Context.Users.ToList();
            var result = this.Mapper.Map<IEnumerable<AdministrateUserViewModel>>(usersWithTopics);
            return result;
        }

        public AdministrateUserViewModel GetUserWithTopics(string userId)
        {
            var userWithTopics = this.Context.Users.Include(u => u.Topics).FirstOrDefault(u => u.Id == userId);
            AdministrateUserViewModel result = null;

            if (userWithTopics != null)
            {
                result = this.Mapper.Map<AdministrateUserViewModel>(userWithTopics);
            }

            return result;
        }
    }
}