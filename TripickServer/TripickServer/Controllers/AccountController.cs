using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Models;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> logger;
        private UserManager<AppUser> userManager { get; }
        private SignInManager<AppUser> signInManager { get; }

        public AccountController(ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("test")]
        public IActionResult test()
        {
            return Ok("Request received");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RequestRegister credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return BadRequest("Invalid credentials.");

            if (credentials.Password != credentials.ConfirmPassword)
                return BadRequest("Passwords don't match.");

            AppUser newUser = new AppUser
            {
                Email = credentials.Email,
                UserName = credentials.Email.Substring(0, credentials.Email.IndexOf('@')).ToCleanUsername(),
                FirstName = string.Empty,
                LastName = string.Empty,
                Photo = string.Empty
            };

            IdentityResult userCreationResult = null;
            try
            {
                userManager.Options.SignIn.RequireConfirmedEmail = false;
                userCreationResult = await userManager.CreateAsync(newUser, credentials.Password);
            }
            catch (SqlException)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Server error, please try again.");
            }

            if (!userCreationResult.Succeeded)
            {
                return BadRequest($"- {string.Join($"{Environment.NewLine}- ", userCreationResult.Errors.Select(x => x.Description).ToList())}");
            }

            // If a confirmation is required
            if (Constants.AuthenticationConfirmEmail)
            {
                //string emailConfirmationToken = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                //string callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { userId = newUser.Id, code = emailConfirmationToken }, protocol: Request.Scheme);
                //await emailSender.SendEmailAsync(credentials.Email, "Tripick : Confirm your account",
                //$"Please confirm your Tripick account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                //await signInManager.SignInAsync(newUser, isPersistent: false);
                //return Ok($"Registration completed, please verify your email - {newUser.Email}");
            }

            return Ok($"Registration successful.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return BadRequest("Invalid credentials.");

            AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return BadRequest("Incorrect credential.");

            await Logout();

            if (Constants.AuthenticationConfirmEmail)
            {
                if (!user.EmailConfirmed)
                    return BadRequest("Email not confirmed, please check your email for confirmation link.");
            }

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return BadRequest("Incorrect credential.");

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            Response.Cookies.Delete(Constants.AuthenticationTokenCookieName);
            return Ok("Log out successful.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return BadRequest("Invalid credentials.");

           AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return BadRequest("Incorrect credential.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return BadRequest("Incorrect credential.");

            await userManager.DeleteAsync(user);
            return Ok("Account deleted successfully.");
        }
    }
}
