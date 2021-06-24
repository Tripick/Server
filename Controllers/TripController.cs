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
    public class TripController : BaseController
    {
        #region Properties

        private readonly ManagerTrip managerTrip;
        private readonly ManagerFilter managerFilter;

        #endregion

        #region Constructor

        public TripController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerTrip = new ManagerTrip(logger, () => this.ConnectedUser, tripickContext);
            this.managerFilter = new ManagerFilter(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Create")]
        public JsonResult Create([FromBody] Request<string> request)
        {
            return managerTrip.SafeCall(() => managerTrip.Create());
        }

        [HttpPost]
        [Route("GetAll")]
        public JsonResult GetAll([FromBody] Request<RequestGetAll> request)
        {
            if (request.Data == null)
                return Error("Trip - GetAll : Data required.");
            return managerTrip.SafeCall(() => managerTrip.GetAll(request.Data.PageIndex, request.Data.PageSize));
        }

        [HttpPost]
        [Route("Get")]
        public JsonResult Get([FromBody] Request<RequestGet> request)
        {
            if (request.Data == null)
                return Error("Trip - Get : Data required.");
            return managerTrip.SafeCall(() => managerTrip.GetById(request.Data.Id));
        }

        [HttpPost]
        [Route("Update")]
        public JsonResult Update([FromBody] Request<RequestUpdate> request)
        {
            if (request.Data == null)
                return Error("Trip - Update : Data required.");
            return managerTrip.SafeCall(() => managerTrip.Update(request.Data.Trip));
        }

        [HttpPost]
        [Route("Delete")]
        public JsonResult Delete([FromBody] Request<RequestDelete> request)
        {
            if (request.Data == null)
                return Error("Trip - Delete : Data required.");
            return managerTrip.SafeCallValueType(() => managerTrip.Delete(request.Data.Id));
        }

        [HttpPost]
        [Route("SaveCover")]
        public JsonResult SaveCover([FromBody] Request<RequestSaveCover> request)
        {
            if (request.Data == null)
                return Error("Trip - SaveCover : Data required.");
            return managerTrip.SafeCallValueType(() => managerTrip.SaveCover(request.Data.IdTrip, request.Data.Cover));
        }

        [HttpPost]
        [Route("GetFilters")]
        public JsonResult GetFilters([FromBody] Request<RequestGet> request)
        {
            if (request.Data == null)
                return Error("Trip - GetFilters : Data required.");
            return managerFilter.SafeCall(() => managerFilter.Get(request.Data.Id));
        }

        [HttpPost]
        [Route("SaveFilters")]
        public JsonResult SaveFilters([FromBody] Request<RequestSaveFilters> request)
        {
            if (request.Data == null)
                return Error("Trip - SaveFilters : Data required.");
            return managerFilter.SafeCall(() => managerFilter.Save(request.Data.IdTrip, request.Data.Filters));
        }


        [HttpPost]
        [Route("GetItinerary")]
        public JsonResult GetItinerary([FromBody] Request<RequestGetItinerary> request)
        {
            if (request.Data == null)
                return Error("Trip - GetItinerary : Data required.");
            return managerTrip.SafeCall(() => managerTrip.GetItinerary(request.Data.IdTrip, request.Data.Regenerate));
        }

    }
}
