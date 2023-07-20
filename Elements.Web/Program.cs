using System;
using Elements.Data;
using Elements.Services.Admin.Interfaces;
using Elements.Services.Admin;
using Elements.Services.Public.Interfaces;
using Elements.Services.Public;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Elements.Models;
using Microsoft.AspNetCore.Identity;
using Elements.Common;
using Elements.Web.Areas.Identity.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Elements.Web.Common;

namespace Elements.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(Constants.PolicyRequireAdminRole, policy => policy.RequireRole(Constants.AdminRoleName, Constants.CreatorRoleName));
                options.AddPolicy(Constants.PolicyRequireAdminDevRole, policy => policy.RequireRole(Constants.AdminRoleName, Constants.CreatorRoleName, Constants.DevRoleName));
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc();
            builder.Services.AddRazorPages(options =>
            {
                //options.Conventions.AuthorizeAreaFolder(Constants.AdminAreaName, "/Admin", Constants.PolicyRequireAdminRole);
                //options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                //options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                //options.Conventions.AddPageRoute("/News", "");
            });

            builder.Services.AddDbContext<ElementsContext>();

            RegisterIdentity(builder.Services);
            RegisterExternalServices(builder.Services);
            RegisterServiceLayer(builder.Services);

            builder.Services
                .AddAuthentication()
                .AddCookie();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;

                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            var dateTimeService = app.Services.GetRequiredService<IDateTimeService>();
            app.SeedDatabase(dateTimeService);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapAreaControllerRoute(
                name: "Admin",
                areaName: "admin",
                pattern: "admin/{controller=Forum}/{action=ManageCategories}"
            );

            app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Forum}/{action=Index}/{id?}");

            // required for razor pages to work
            app.MapRazorPages();
            app.Run();
        }

        static void RegisterIdentity(IServiceCollection services)
        {
            // set up app to use User and IdentityRole
            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ElementsContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequireDigit = false,
                    RequiredLength = 4,
                    RequiredUniqueChars = 1,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false
                };

                options.SignIn.RequireConfirmedEmail = false;
            });
        }

        static void RegisterExternalServices(IServiceCollection services)
        {
            services.AddAutoMapper();
            services.AddSingleton<IEmailSender, SendGridEmailSender>();
        }

        static void RegisterServiceLayer(IServiceCollection services)
        {
            services.AddScoped<IManageCategoriesService, ManageCategoriesService>();
            services.AddScoped<IManageUsersService, ManageUsersService>();
            services.AddScoped<IManageNewsService, ManageNewsService>();

            // public services
            services.AddTransient<IDateTimeService, DateTimeService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ITopicService, TopicService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICharacterService, CharacterService>();
        }
    }
}
