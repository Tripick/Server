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
using TripickServer.Requests.Friend;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FriendController : BaseController
    {
        #region Properties

        private readonly ManagerFriend managerFriend;

        #endregion

        #region Constructor

        public FriendController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerFriend = new ManagerFriend(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Add")]
        public JsonResult Add([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Friend - Add : Data required.");
            return managerFriend.SafeCall(() => managerFriend.Add(request.Data.Id));
        }

        [HttpPost]
        [Route("Confirm")]
        public JsonResult Confirm([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Friend - Confirm : Data required.");
            return managerFriend.SafeCall(() => managerFriend.Confirm(request.Data.Id));
        }

        [HttpPost]
        [Route("Delete")]
        public JsonResult Delete([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Friend - Delete : Data required.");
            return managerFriend.SafeCallValueType(() => managerFriend.Delete(request.Data.Id));
        }

        [HttpPost]
        [Route("Search")]
        public JsonResult Search([FromBody] Request<RequestSearch> request)
        {
            if (request.Data == null)
                return Error("Friend - Search : Data required.");
            return managerFriend.SafeCall(() => managerFriend.Search(request.Data.UserName));
        }
    }
}
