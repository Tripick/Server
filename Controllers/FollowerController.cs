using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.Follower;
using TripickServer.Requests.Trip;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FollowerController : BaseController
    {
        #region Properties

        private readonly ManagerFollower managerFollower;

        #endregion

        #region Constructor

        public FollowerController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerFollower = new ManagerFollower(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Add")]
        public JsonResult Add([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Follower - Add : Data required.");
            return managerFollower.SafeCall(() => managerFollower.Add(request.Data.IdTrip, request.Data.IdFriend));
        }

        [HttpPost]
        [Route("Delete")]
        public JsonResult Delete([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Follower - Delete : Data required.");
            return managerFollower.SafeCallValueType(() => managerFollower.Delete(request.Data.IdTrip, request.Data.IdFriend));
        }
    }
}
