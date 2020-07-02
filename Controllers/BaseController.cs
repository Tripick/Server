using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TripickServer.Models;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ServiceFilter(typeof(CheckAuthKeysAndConnect))]
    public class BaseController : ControllerBase
    {
        #region Properties

        protected readonly ILogger<ServerLogger> Logger;
        private UserManager<AppUser> userManager;
        protected AppUser ConnectedUser { get; private set; }

        #endregion

        #region Constructor

        public BaseController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager)
        {
            this.Logger = logger;
            this.userManager = userManager;
        }

        #endregion

        #region Methods

        public JsonResult Error(string error)
        {
            return ServerResponse<string>.ToJson(false, error);
        }

        public async Task<bool> ConnectCurrentUser(AuthenticationKeys authenticationKeys)
        {
            if (authenticationKeys == null || string.IsNullOrWhiteSpace(authenticationKeys.AccessToken))
                return false;

            AppUser user = await userManager.FindByIdAsync(authenticationKeys.Id.ToString());
            if (user == null)
                return false;

            string existingToken = await userManager.GetAuthenticationTokenAsync(user, Constants.AppName, Constants.AuthenticationTokenName);
            bool authenticationKeysValid = existingToken == authenticationKeys.AccessToken;

            if (authenticationKeysValid)
                this.ConnectedUser = user;

            return authenticationKeysValid;
        }

        #endregion
    }
}
