using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.Country;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountryController : BaseController
    {
        #region Properties

        private readonly ManagerCountry managerCountry;

        #endregion

        #region Constructor

        public CountryController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerCountry = new ManagerCountry(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Get")]
        public JsonResult Get([FromBody] Request<RequestGet> request)
        {
            if (request.Data == null || !request.Data.Id.HasValue || string.IsNullOrWhiteSpace(request.Data.Name))
                return Error("Country - Get : Data required.");
            return managerCountry.SafeCall(() => managerCountry.Get(request.Data.Id, request.Data.Name));
        }

        [HttpPost]
        [Route("GetComplete")]
        public JsonResult GetComplete([FromBody] Request<RequestGet> request)
        {
            if (request.Data == null || !request.Data.Id.HasValue)
                return Error("Country - GetComplete : Data required.");
            return managerCountry.SafeCall(() => managerCountry.GetComplete(request.Data.Id.Value));
        }

        [HttpPost]
        [Route("GetByLocation")]
        public JsonResult GetByLocation([FromBody] Request<RequestGetByLocation> request)
        {
            if (request.Data == null)
                return Error("Country - GetByLocation : Data required.");
            return managerCountry.SafeCall(() => managerCountry.GetByLocation(request.Data.Latitude, request.Data.Longitude));
        }

        [HttpPost]
        [Route("GetAll")]
        public JsonResult GetAll([FromBody] Request<RequestGetAll> request)
        {
            return managerCountry.SafeCall(() => managerCountry.GetAll());
        }


        // TODO Hugo : remove acess to this function after generating countries !!!
        [HttpPost]
        [Route("GenerationGetAll")]
        public JsonResult GenerationGetAll([FromBody] Request<RequestGetAll> request)
        {
            return managerCountry.SafeCall(() => managerCountry.GenerationGetAll(request.Data.Quantity));
        }

        // TODO Hugo : remove acess to this function after generating countries !!!
        [HttpPost]
        [Route("GenerationSave")]
        public JsonResult GenerationSave([FromBody] Request<RequestSave> request)
        {
            return managerCountry.SafeCall(() => managerCountry.GenerationSave(request.Data.Country));
        }
    }
}
