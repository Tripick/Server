using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.Pick;
using TripickServer.Requests.Trip;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PickController : BaseController
    {
        #region Properties

        private readonly ManagerPick managerPick;

        #endregion

        #region Constructor

        public PickController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerPick = new ManagerPick(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("GetAll")]
        public JsonResult GetAll([FromBody] Request<RequestGetAllPicks> request)
        {
            if (request.Data == null)
                return Error("Pick - GetAll : Data required.");
            return managerPick.SafeCall(() => managerPick.GetAll(request.Data.IdTrip, request.Data.Quantity, request.Data.Skip));
        }

        [HttpPost]
        [Route("GetNexts")]
        public JsonResult GetNexts([FromBody] Request<RequestGetNext> request)
        {
            if (request.Data == null)
                return Error("Pick - GetNexts : Data required.");
            return managerPick.SafeCall(() => managerPick.GetNexts(request.Data.IdTrip, request.Data.Quantity, request.Data.AlreadyLoaded));
        }

        [HttpPost]
        [Route("SavePick")]
        public JsonResult SavePick([FromBody] Request<RequestSavePick> request)
        {
            if (request.Data == null)
                return Error("Pick - SavePick : Data required.");
            return managerPick.SafeCallValueType(() => managerPick.SavePick(request.Data.IdTrip, request.Data.IdPlace, request.Data.Rating));
        }
    }
}
