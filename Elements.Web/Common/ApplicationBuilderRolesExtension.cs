namespace Elements.Web.Common
{
    using Elements.Common;
    using Elements.Models;
    using Elements.Services.Public.Interfaces;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class ApplicationBuilderRolesExtension
    {
        private static Tuple<string, string, string, string> masterUser = new Tuple<string, string, string, string>("master", "master@admins.com", "secret", "8f3151bb-62d1-47b7-924a-9901bab02f60");
        private static Tuple<string, string, string, string> adminUser = new Tuple<string, string, string, string>("admin", "admin@admins.com", "admin123", "86242202-572c-424d-af07-ddd41f608b96");
        private static Tuple<string, string, string, string> devUser = new Tuple<string, string, string, string>("dev", "dev@dev.com", "dev123", "67b787af-140a-4968-8c86-6168e88723bb");
        private static Tuple<string, string, string, string> modUser = new Tuple<string, string, string, string>("mod", "mod@admins.com", "mod123", "c01ddea6-e3c6-4fe8-8fca-feb554a12ebe");
        private static Tuple<string, string, string, string> user = new Tuple<string, string, string, string>("user", "user@example.jm", "123qwe", "79c5d801-7895-4c35-bace-9f8115386825");

        private static IReadOnlyDictionary<string, HashSet<Tuple<string, string, string, string>>> defaultUsers =
            new Dictionary<string, HashSet<Tuple<string, string, string, string>>>()
        {
            {
                Constants.CreatorRoleName,
                new HashSet<Tuple<string, string, string, string>>(){
                    masterUser
                }
            },
        {
                Constants.AdminRoleName,
                new HashSet<Tuple<string, string, string, string>>(){
                    masterUser,
                    adminUser
                }
            },
        {
                Constants.DevRoleName,
                new HashSet<Tuple<string, string, string, string>>(){
                    masterUser,
                    devUser
                }
            },
        {
                Constants.ModeratorRoleName,
                new HashSet<Tuple<string, string, string, string>>(){
                    masterUser,
                    modUser
                }
            },
        {
                Constants.UserRoleName,
                new HashSet<Tuple<string, string, string, string>>(){
                    masterUser,
                    user
                }
            },
        };

        private static readonly IReadOnlyDictionary<string, HashSet<User>> roles = new Dictionary<string, HashSet<User>>()
        {
            { Constants.CreatorRoleName, new HashSet<User>() },
            { Constants.AdminRoleName, new HashSet<User>() },
            { Constants.DevRoleName, new HashSet<User>() },
            { Constants.ModeratorRoleName, new HashSet<User>() },
            { Constants.UserRoleName, new HashSet<User>() },
        };

        public static async void SeedDatabase(this IApplicationBuilder app, IDateTimeService dateTimeService)
        {
            var serviceScoreFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            var scope = serviceScoreFactory.CreateScope();
            using (scope)
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // add the roles if they dont exist
                await SeedRolesAsync(roleManager);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                // add default administrator if it doesnt exist
                await SeedUsers(userManager, dateTimeService);
            }
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in roles.Keys)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var reuslt = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task SeedUsers(UserManager<User> userManager, IDateTimeService dateTimeService)
        {
            foreach (var role in defaultUsers.Keys)
            {
                foreach (var currentUser in defaultUsers[role])
                {
                    string username = currentUser.Item1;

                    var user = await userManager.FindByNameAsync(username);

                    if (user == null)
                    {
                        string email = currentUser.Item2;
                        string password = currentUser.Item3;
                        string id = currentUser.Item4;

                        user = new User()
                        {
                            Id = id,
                            UserName = username,
                            Email = email,
                            RegisterDate = dateTimeService.Now,
                            Avatar = Constants.UserRoleName
                        };

                        var result = await userManager.CreateAsync(user, password);
                        result = await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}
