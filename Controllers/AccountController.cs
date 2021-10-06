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
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<ServerLogger> logger;

        private UserManager<AppUser> userManager;
        private ManagerAccount accountManager;
        private SignInManager<AppUser> signInManager { get; }

        public AccountController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            TripickContext tripickContext)
        {
            this.logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.accountManager = new ManagerAccount(logger, () => null, userManager, signInManager, tripickContext);
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
            if (credentials == null)
                return ServerResponse<UserContext>.ToJson(false, "Credentials required.");
            return await accountManager.SafeCallAsync(async () => 
                await accountManager.Register(credentials.Email, credentials.Username, credentials.Password, credentials.ConfirmPassword));
        }

        [HttpPost]
        [Route("Login")]
        public async Task<JsonResult> Login([FromBody] RequestLogin credentials)
        {
            if (credentials == null)
                return ServerResponse<UserContext>.ToJson(false, "Credentials required.");
            return await accountManager.SafeCallAsync(async () => await accountManager.Login(credentials.Email, credentials.Password, credentials.RememberMe));
        }

        [HttpPost]
        [Route("LoginByToken")]
        public async Task<JsonResult> LoginByToken([FromBody] Request<string> request)
        {
            if (request == null || request.AuthenticationKeys == null)
                return ServerResponse<UserContext>.ToJson(false, "Credentials required.");
            return await accountManager.SafeCallAsync(async () => await accountManager.LoginByToken(request.AuthenticationKeys.Id, request.AuthenticationKeys.AccessToken));
        }

        [HttpPost]
        [Route("Logout")]
        public async Task<JsonResult> Logout([FromBody] Request<string> request)
        {
            if (request == null || request.AuthenticationKeys == null)
                return ServerResponse<UserContext>.ToJson(false, "Credentials required.");
            return await accountManager.SafeCallValueTypeAsync(async () => await accountManager.Logout(request.AuthenticationKeys.Id, request.AuthenticationKeys.AccessToken));
        }

        [HttpPost]
        [Route("Reset")]
        public async Task<JsonResult> Reset([FromBody] RequestReset request)
        {
            if (request == null)
                return ServerResponse<UserContext>.ToJson(false, "Reset : Email required.");
            return await accountManager.SafeCallValueTypeAsync(async () => await accountManager.Reset(request.Email));
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<JsonResult> Delete([FromBody] RequestLogin credentials)
        {
            if (credentials == null)
                return ServerResponse<UserContext>.ToJson(false, "Credentials required.");
            return await accountManager.SafeCallValueTypeAsync(async () => await accountManager.Delete(credentials.Email, credentials.Password, credentials.RememberMe));
        }
    }
}
