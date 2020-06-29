using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using TripickServer.Models;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        #region Properties

        protected readonly ILogger<ServerLogger> Logger;
        protected ClaimsPrincipal ConnectedUserClaim;
        private AppUser user;
        protected AppUser ConnectedUser
        {
            get
            {
                if (user == null)
                {
                    user = new AppUser()
                    {
                        Id = int.Parse(ConnectedUserClaim.GetUserId()),
                        UserName = ConnectedUserClaim.GetUserName(),
                        Email = ConnectedUserClaim.GetUserEmail()
                    };
                }
                return this.user;
            }
        }

        #endregion

        #region Constructor

        public BaseController(
            IHttpContextAccessor contextAccessor,
            ILogger<ServerLogger> logger)
        {
            this.ConnectedUserClaim = contextAccessor.HttpContext.User;
            this.Logger = logger;
        }

        #endregion

        #region Methods

        protected IActionResult SafeCall<T>(Func<T> call)
        {
            ServerResponse<T> response = new ServerResponse<T>();
            try
            {
                T result = call();
                response.IsSuccess = true;
                response.Result = result;
                response.Message = string.Empty;
                return Ok(response);
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Result = default(T);
                response.Message = e.Message;
                return BadRequest(response);
            }
        }

        #endregion
    }
}
