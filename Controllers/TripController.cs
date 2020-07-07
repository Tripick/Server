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

        #endregion

        #region Constructor

        public TripController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerTrip = new ManagerTrip(logger, () => this.ConnectedUser, tripickContext);
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
            return managerTrip.SafeCall(() => managerTrip.GetById(request.Data.Id, false));
        }

        [HttpPost]
        [Route("GetFull")]
        public JsonResult GetFull([FromBody] Request<RequestGet> request)
        {
            if (request.Data == null)
                return Error("Trip - Get : Data required.");
            return managerTrip.SafeCall(() => managerTrip.GetById(request.Data.Id, true));
        }

        [HttpPost]
        [Route("Update")]
        public JsonResult Update([FromBody] Request<RequestUpdate> request)
        {
            if (request.Data == null)
                return Error("Trip - Get : Data required.");
            return managerTrip.SafeCall(() => managerTrip.Update(request.Data.Trip));
        }

        [HttpPost]
        [Route("Delete")]
        public JsonResult Delete([FromBody] Request<RequestDelete> request)
        {
            if (request.Data == null)
                return Error("Trip - Get : Data required.");
            return managerTrip.SafeCall(() => managerTrip.Delete(request.Data.Id));
        }
    }
}
