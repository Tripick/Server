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

        [HttpGet]
        public JsonResult ConnectionError(string error)
        {
            return ServerResponse<string>.ToJson(error);
        }

        [HttpGet]
        [Route("test")]
        public JsonResult test()
        {
            return ServerResponse<string>.ToJson("test success.");
        }

        [HttpPost]
        [Route("testpost")]
        public JsonResult testpost([FromBody] Testpost testpost)
        {
            return ServerResponse<string>.ToJson("testpost success.");
        }

        [HttpPost]
        [Route("Register")]
        public async Task<JsonResult> Register([FromBody] RequestRegister credentials)
        {
            if (credentials == null ||
                string.IsNullOrWhiteSpace(credentials.Email) ||
                string.IsNullOrWhiteSpace(credentials.Username) ||
                string.IsNullOrWhiteSpace(credentials.Password))
                return ServerResponse<UserContext>.ToJson(false, "Invalid credentials.");

            if (credentials.Password != credentials.ConfirmPassword)
                return ServerResponse<UserContext>.ToJson(false, "Passwords don't match.");

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
                return ServerResponse<UserContext>.ToJson(false, "Server error, please try again.");
            }
            catch (Exception e)
            {
                return ServerResponse<UserContext>.ToJson(false, "Server error, please try again : " + e.Message);
            }

            if (!userCreationResult.Succeeded)
                return ServerResponse<UserContext>.ToJson(false, $"- {string.Join($"{Environment.NewLine}- ", userCreationResult.Errors.Select(x => x.Description).ToList())}");

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

        [HttpPost]
        [Route("Login")]
        public async Task<JsonResult> Login([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return ServerResponse<UserContext>.ToJson(false, "Invalid credentials.");

            AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return ServerResponse<UserContext>.ToJson(false, "This account doesn't exist.");

            await signInManager.SignOutAsync();

            if (Constants.AuthenticationConfirmEmail && !user.EmailConfirmed)
                return ServerResponse<UserContext>.ToJson(false, "Email not confirmed, please check your email for confirmation link.");

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return ServerResponse<UserContext>.ToJson(false, "Wrong login or password.");

            // Generate new token
            await userManager.RemoveAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            var newToken = await userManager.GenerateUserTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            await userManager.SetAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName, newToken);

            // Send User and AuthenticationKeys
            ServerResponse<UserContext> response = new ServerResponse<UserContext>(new UserContext(user, HttpUtility.UrlEncode(newToken)));
            return new JsonResult(response);
        }

        [HttpPost]
        [Route("LoginByToken")]
        public async Task<JsonResult> LoginByToken([FromBody] Request<string> request)
        {
            if (request == null || request.AuthenticationKeys == null || string.IsNullOrWhiteSpace(request.AuthenticationKeys.AccessToken))
                return ServerResponse<UserContext>.ToJson(false, "Token required.");

            AppUser user = await userManager.FindByIdAsync(request.AuthenticationKeys.Id.ToString());
            if (user == null)
                return ServerResponse<string>.ToJson(false, "This account doesn't exist.");

            string existingToken = await userManager.GetAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            bool tokenIsValid = existingToken == HttpUtility.UrlDecode(request.AuthenticationKeys.AccessToken);
            if(tokenIsValid)
            {
                await signInManager.SignInAsync(user, isPersistent: true);
                return ServerResponse<UserContext>.ToJson(new UserContext(user, request.AuthenticationKeys.AccessToken));
            }
            else
            {
                return ServerResponse<UserContext>.ToJson(false, "Token invalid or expired.");
            }
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<JsonResult> Logout([FromBody] Request<string> request)
        {
            if (request == null || request.AuthenticationKeys == null || string.IsNullOrWhiteSpace(request.AuthenticationKeys.AccessToken))
                return ServerResponse<UserContext>.ToJson(false, "Token required.");

            AppUser user = await userManager.FindByIdAsync(request.AuthenticationKeys.Id.ToString());
            if (user == null)
                return ServerResponse<string>.ToJson(false, "This account doesn't exist.");

            // Generate new token
            await userManager.RemoveAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            var newToken = await userManager.GenerateUserTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            await userManager.SetAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName, newToken);

            return ServerResponse<string>.ToJson("Logged out.");
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<JsonResult> Delete([FromBody] RequestLogin credentials)
        {
            if (credentials == null || string.IsNullOrWhiteSpace(credentials.Email) || string.IsNullOrWhiteSpace(credentials.Password))
                return ServerResponse<string>.ToJson(false, "Invalid credentials.");

           AppUser user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
                return ServerResponse<string>.ToJson(false, "This account doesn't exist.");

            await signInManager.SignOutAsync();

            Microsoft.AspNetCore.Identity.SignInResult passwordSignInResult = await signInManager.PasswordSignInAsync(user, credentials.Password, isPersistent: true, lockoutOnFailure: false);
            if (!passwordSignInResult.Succeeded)
                return ServerResponse<string>.ToJson(false, "Incorrect credentials.");

            await userManager.DeleteAsync(user);
            return ServerResponse<string>.ToJson( "Account deleted.");
        }
    }
}
