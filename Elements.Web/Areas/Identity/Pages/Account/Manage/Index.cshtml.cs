using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Elements.Models;
using Elements.Services.Public.Interfaces;
using Elements.Web.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Elements.Web.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IUserService userService;

        // ~500kb
        private const int DefaultFileSize = 500000;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IUserService userService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._emailSender = emailSender;
            this.userService = userService;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            public string Avatar { get; set; }

            public IFormFile ImageFile { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var avatar = user.Avatar;

            this.Username = userName;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,
                Avatar = avatar
            };

            this.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }

            if (this.Input.ImageFile != null)
            {
                if (CustomValidator.IsFormFileLenghtBiggerThan(this.Input.ImageFile, DefaultFileSize))
                {
                    throw new InvalidOperationException($"Please select smaller image(jpeg or png)");
                }

                if (!CustomValidator.IsFormFileInFormat(this.Input.ImageFile, "image/png", "image/jpeg", "image/jpg"))
                {
                    throw new InvalidOperationException($"Please select a valid image file (jpeg, jpg or png)");
                }

                var fileName = user.UserName + "." + this.Input.ImageFile.ContentType.Substring("image/".Length);
                var avatarUrl = ImageManager.GetIconRelativePath("avatars", fileName);

                this.userService.SetAvatar(user.Id, avatarUrl);

                var fullFilePathName = ImageManager.GetFullFilePath("avatars", fileName);
                using (var fileStream = new FileStream(fullFilePathName, FileMode.Create))
                {
                    await this.Input.ImageFile.CopyToAsync(fileStream);
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
