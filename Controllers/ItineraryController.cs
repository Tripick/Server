using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.Trip;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItineraryController : BaseController
    {
        #region Properties

        private readonly ManagerItinerary managerItinerary;
        private readonly ManagerPick managerPick;

        #endregion

        #region Constructor

        public ItineraryController(ILogger<ServerLogger> logger, UserManager<AppUser> userManager, TripickContext tripickContext) : base(logger, userManager)
        {
            this.managerItinerary = new ManagerItinerary(logger, () => this.ConnectedUser, tripickContext);
            this.managerPick = new ManagerPick(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Get")]
        public JsonResult Get([FromBody] Request<RequestGetItinerary> request)
        {
            if (request.Data == null)
                return Error("Itinerary - GetItinerary : Data required.");
            return managerItinerary.SafeCall(() => managerItinerary.GetItinerary(request.Data.IdTrip, request.Data.ForceRegeneration));
        }

        [HttpPost]
        [Route("Delete")]
        public JsonResult Delete([FromBody] Request<RequestDeleteItinerary> request)
        {
            if (request.Data == null)
                return Error("Itinerary - Delete : Data required.");
            return managerItinerary.SafeCallValueType(() => managerItinerary.Delete(request.Data.IdTrip));
        }

        [HttpPost]
        [Route("SaveDays")]
        public JsonResult SaveDays([FromBody] Request<RequestSaveDays> request)
        {
            if (request.Data == null)
                return Error("Itinerary - SaveDays : Data required.");
            return managerItinerary.SafeCall(() => managerItinerary.SaveDays(request.Data.IdTrip, request.Data.Days));
        }

        [HttpPost]
        [Route("SaveDay")]
        public JsonResult SaveDay([FromBody] Request<RequestSaveDay> request)
        {
            if (request.Data == null)
                return Error("Itinerary - SaveDay : Data required.");
            return managerItinerary.SafeCallValueType(() => managerItinerary.SaveDay(request.Data.IdTrip, request.Data.Day));
        }

        [HttpPost]
        [Route("SaveSteps")]
        public JsonResult SaveSteps([FromBody] Request<RequestSaveSteps> request)
        {
            if (request.Data == null)
                return Error("Itinerary - SaveSteps : Data required.");
            return managerItinerary.SafeCall(() => managerItinerary.SaveSteps(request.Data.IdTrip, request.Data.IdDay, request.Data.Steps));
        }

        [HttpPost]
        [Route("SaveStep")]
        public JsonResult SaveStep([FromBody] Request<RequestSaveStep> request)
        {
            if (request.Data == null)
                return Error("Itinerary - SaveStep : Data required.");
            return managerItinerary.SafeCallValueType(() => managerItinerary.SaveStep(request.Data.IdTrip, request.Data.IdDay, request.Data.Step));
        }

        [HttpPost]
        [Route("MoveStep")]
        public JsonResult MoveStep([FromBody] Request<RequestMoveStep> request)
        {
            if (request.Data == null)
                return Error("Itinerary - MoveStep : Data required.");
            return managerItinerary.SafeCallValueType(() => managerItinerary.MoveStep(request.Data.IdTrip, request.Data.IdOldDay, request.Data.IdNewDay, request.Data.IdStep));
        }

        [HttpPost]
        [Route("SearchMap")]
        public JsonResult SearchMap([FromBody] Request<RequestSearchMap> request)
        {
            if (request.Data == null)
                return Error("Itinerary - SearchMap : Data required.");
            return managerItinerary.SafeCall(() => managerPick.SearchMap(request.Data.IdTrip, request.Data.Region));
        }
    }
}
