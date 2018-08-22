using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Elements.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Elements.Services.Models.Account;
using Elements.Common;
using Elements.Services.Public.Interfaces;

namespace Elements.Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;
        private readonly IDateTimeService dateTimeService;

        public RegisterModel(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IDateTimeService dateTimeService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
            this.dateTimeService = dateTimeService;
        }

        [BindProperty]
        public RegisterNewUserModel RegisterNewUserModel { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = RegisterNewUserModel.Username,
                    Email = RegisterNewUserModel.Email,
                    RegisterDate = dateTimeService.Now,
                    Avatar = Constants.DefaultAvatar,
                };

                var result = await this.userManager.CreateAsync(user, RegisterNewUserModel.Password);
                if (result.Succeeded)
                {
                    this.logger.LogInformation(string.Format("User created a new account with password. - {0}", user.UserName));

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await this.emailSender.SendEmailAsync(RegisterNewUserModel.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    await signInManager.SignInAsync(user, isPersistent: false);
                    await userManager.AddToRoleAsync(user, Constants.UserRoleName);

                    return this.LocalRedirect(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.Page();
        }
    }
}
