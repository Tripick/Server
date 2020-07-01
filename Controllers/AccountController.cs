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
using Microsoft.Extensions.Primitives;
using TripickServer.Models;
using TripickServer.Requests;
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
            return Ok("Request received UPGRADED");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("testpost")]
        public IActionResult testpost([FromBody] Testpost testpost)
        {
            return Ok("Request testpost received.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<ServerResponse<AppUser>> Register([FromBody] RequestRegister credentials)
        {
            ServerResponse<AppUser> response = new ServerResponse<AppUser>();

            if (credentials == null ||
                string.IsNullOrWhiteSpace(credentials.Email) ||
                string.IsNullOrWhiteSpace(credentials.Username) ||
                string.IsNullOrWhiteSpace(credentials.Password))
                return new ServerResponse<AppUser>(false, "Invalid credentials.");

            if (credentials.Password != credentials.ConfirmPassword)
                return new ServerResponse<AppUser>(false, "Passwords don't match.");

            AppUser newUser = new AppUser
            {
                Email = credentials.Email,
                UserName = credentials.Username,
                FirstName = string.Empty,
                LastName = string.Empty
            };

            IdentityResult userCreationResult = null;
            try
            {
                userManager.Options.SignIn.RequireConfirmedEmail = false;
                userCreationResult = await userManager.CreateAsync(newUser, credentials.Password);
            }
            catch (SqlException)
            {
                return new ServerResponse<AppUser>(false, "Server error, please try again.");
            }
            catch (Exception e)
            {
                return new ServerResponse<AppUser>(false, "Server error, please try again : " + e.Message);
            }

            if (!userCreationResult.Succeeded)
                return new ServerResponse<AppUser>(false, $"- {string.Join($"{Environment.NewLine}- ", userCreationResult.Errors.Select(x => x.Description).ToList())}");

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

            return await Login(new RequestLogin() { Email = credentials.Email, Password = credentials.Password });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ServerResponse<AppUser>> Login([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return new ServerResponse<AppUser>(false, "Invalid credentials.");

            AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return new ServerResponse<AppUser>(false, "This account doesn't exist.");

            await Logout();

            if (Constants.AuthenticationConfirmEmail && !user.EmailConfirmed)
                return new ServerResponse<AppUser>(false, "Email not confirmed, please check your email for confirmation link.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return new ServerResponse<AppUser>(false, "Wrong login or password.");

            ServerResponse<AppUser> response = new ServerResponse<AppUser>(user);
            StringValues token;
            Response.Headers.TryGetValue(Constants.AuthenticationTokenCookieName, out token);
            response.Message = token.FirstOrDefault();
            return response;
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("Logout")]
        public async Task<ServerResponse<string>> Logout()
        {
            await signInManager.SignOutAsync();
            Response.Cookies.Delete(Constants.AuthenticationTokenCookieName);
            return new ServerResponse<string>("Logged out.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Delete")]
        public async Task<ServerResponse<string>> Delete([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return new ServerResponse<string>(false, "Invalid credentials.");

           AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return new ServerResponse<string>(false, "Incorrect credentials.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return new ServerResponse<string>(false, "Incorrect credentials.");

            await userManager.DeleteAsync(user);
            return new ServerResponse<string>( "Account deleted.");
        }
    }
}
