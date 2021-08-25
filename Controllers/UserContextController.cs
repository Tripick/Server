using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private UserManager<AppUser> userManager;
        private readonly ManagerTrip managerTrip;

        #endregion

        #region Constructor

        public UserContextController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.userManager = userManager;
            this.managerTrip = new ManagerTrip(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Get")]
        public async Task<JsonResult> Get([FromBody] Request<string> request)
        {
            try
            {
                // Get user friends list
                AppUser user = await userManager.Users.Include(x => x.Friendships).SingleAsync(x => x.Id == this.ConnectedUser.Id);
                List<Friend> friends = new List<Friend>();
                if (user != null && user.Friendships != null)
                {
                    List<int> ids = user.Friendships.Select(x => x.IdFriend).ToList();
                    List<AppUser> friendsUsers = userManager.Users.Include(x => x.Photo).Where(x => ids.Contains(x.Id)).ToList();
                    friends = friendsUsers.Select(x => new Friend(x)).ToList();
                }

                // Get all trips
                List<Trip> trips = managerTrip.GetAll(0, int.MaxValue);
                UserContext userContext = new UserContext(managerTrip.LoadConfiguration(), friends, trips, new List<Guide>());
                return ServerResponse<UserContext>.ToJson(userContext?.ToDTO());
            }
            catch (Exception e)
            {
                return ServerResponse<string>.ToJson(false, e.Message);
            }
        }
    }
}
