using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.Traveler;
using TripickServer.Requests.Trip;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TravelerController : BaseController
    {
        #region Properties

        private readonly ManagerTraveler managerTraveler;

        #endregion

        #region Constructor

        public TravelerController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerTraveler = new ManagerTraveler(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Add")]
        public JsonResult Add([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Traveler - Add : Data required.");
            return managerTraveler.SafeCall(() => managerTraveler.Add(request.Data.IdTrip, request.Data.IdFriend));
        }

        [HttpPost]
        [Route("Delete")]
        public JsonResult Delete([FromBody] Request<RequestAdd> request)
        {
            if (request.Data == null)
                return Error("Traveler - Delete : Data required.");
            return managerTraveler.SafeCallValueType(() => managerTraveler.Delete(request.Data.IdTrip, request.Data.IdFriend));
        }
    }
}
