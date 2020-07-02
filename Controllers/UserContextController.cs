using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.UserContext;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserContextController : BaseController
    {
        #region Properties

        private readonly ManagerTrip managerTrip;

        #endregion

        #region Constructor

        public UserContextController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerTrip = new ManagerTrip(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Get")]
        public JsonResult Get([FromBody] Request<string> request)
        {
            try
            {
                List<Trip> trips = managerTrip.GetAll();
                UserContext userContext = new UserContext(trips, new List<Guide>());
                return ServerResponse<UserContext>.ToJson(userContext);
            }
            catch (Exception e)
            {
                return ServerResponse<string>.ToJson(false, e.Message);
            }
        }
    }
}
