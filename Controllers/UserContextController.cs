﻿using Microsoft.AspNetCore.Identity;
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
                List<Friend> friends = new List<Friend>();
                AppUser user = await userManager.Users.Include(x => x.Friendships).SingleAsync(x => x.Id == this.ConnectedUser.Id);
                if (user != null && user.Friendships != null)
                {
                    List<int> ids = user.Friendships.Select(x => x.FriendId).ToList();
                    List<AppUser> friendsUsers = userManager.Users.Include(x => x.Photo).Where(x => ids.Contains(x.Id)).ToList();
                    friends = friendsUsers.Select(x => new Friend(x)).ToList();
                }

                List<Trip> trips = managerTrip.GetAll();
                UserContext userContext = new UserContext(friends, trips, new List<Guide>());
                return ServerResponse<UserContext>.ToJson(userContext);
            }
            catch (Exception e)
            {
                return ServerResponse<string>.ToJson(false, e.Message);
            }
        }
    }
}
