using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        private UserManager<AppUser> userManager;
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
        public JsonResult test()
        {
            return ServerResponse<string>.ToJson("test success.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("testpost")]
        public JsonResult testpost([FromBody] Testpost testpost)
        {
            return ServerResponse<string>.ToJson("testpost success.");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<JsonResult> Register([FromBody] RequestRegister credentials)
        {
            if (credentials == null ||
                string.IsNullOrWhiteSpace(credentials.Email) ||
                string.IsNullOrWhiteSpace(credentials.Username) ||
                string.IsNullOrWhiteSpace(credentials.Password))
                return ServerResponse<AppUser>.ToJson(false, "Invalid credentials.");

            if (credentials.Password != credentials.ConfirmPassword)
                return ServerResponse<AppUser>.ToJson(false, "Passwords don't match.");

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
                return ServerResponse<AppUser>.ToJson(false, "Server error, please try again.");
            }
            catch (Exception e)
            {
                return ServerResponse<AppUser>.ToJson(false, "Server error, please try again : " + e.Message);
            }

            if (!userCreationResult.Succeeded)
                return ServerResponse<AppUser>.ToJson(false, $"- {string.Join($"{Environment.NewLine}- ", userCreationResult.Errors.Select(x => x.Description).ToList())}");

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

            return new JsonResult(await Login(new RequestLogin() { Email = credentials.Email, Password = credentials.Password }));
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<JsonResult> Login([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return ServerResponse<AppUser>.ToJson(false, "Invalid credentials.");

            AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return ServerResponse<AppUser>.ToJson(false, "This account doesn't exist.");

            await Logout();

            if (Constants.AuthenticationConfirmEmail && !user.EmailConfirmed)
                return ServerResponse<AppUser>.ToJson(false, "Email not confirmed, please check your email for confirmation link.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return ServerResponse<AppUser>.ToJson(false, "Wrong login or password.");

            ServerResponse<AppUser> response = new ServerResponse<AppUser>(user);
            await userManager.RemoveAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            var newToken = await userManager.GenerateUserTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            await userManager.SetAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName, newToken);
            response.Message = HttpUtility.UrlEncode(newToken);
            return new JsonResult(response);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("LoginByToken")]
        public async Task<JsonResult> LoginByToken([FromBody] RequestLoginByToken credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Token))
                return ServerResponse<AppUser>.ToJson(false, "Token required.");

            AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return ServerResponse<string>.ToJson(false, "This account doesn't exist.");

            await Logout();

            string existingToken = await userManager.GetAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            bool tokenIsValid = existingToken == HttpUtility.UrlDecode(credentials.Token);
            if(tokenIsValid)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                return ServerResponse<AppUser>.ToJson(user);
            }
            else
            {
                return ServerResponse<AppUser>.ToJson(false, "Token invalid or expired.");
            }
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("Logout")]
        public async Task<JsonResult> Logout()
        {
            if(this.User != null && signInManager.IsSignedIn(this.User))
            {
                AppUser user = await userManager.FindByIdAsync(userManager.GetUserId(this.User));
                if (user == null)
                    return ServerResponse<string>.ToJson(false, "This account doesn't exist.");
                await userManager.UpdateSecurityStampAsync(user);
            }

            await signInManager.SignOutAsync();
            Response.Cookies.Delete(Constants.AuthenticationTokenName);
            return ServerResponse<string>.ToJson("Logged out.");
        }

        [AllowAnonymous]
        [HttpDelete]
        [Route("Delete")]
        public async Task<JsonResult> Delete([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return ServerResponse<string>.ToJson(false, "Invalid credentials.");

           AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return ServerResponse<string>.ToJson(false, "This account doesn't exist.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return ServerResponse<string>.ToJson(false, "Incorrect credentials.");

            await userManager.DeleteAsync(user);
            return ServerResponse<string>.ToJson( "Account deleted.");
        }
    }
}
