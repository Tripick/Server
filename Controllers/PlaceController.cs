using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
using TripickServer.Requests.Place;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaceController : BaseController
    {
        #region Properties

        private readonly ManagerPlace managerPlace;

        #endregion

        #region Constructor

        public PlaceController(
            ILogger<ServerLogger> logger,
            UserManager<AppUser> userManager,
            TripickContext tripickContext)
        : base(logger, userManager)
        {
            this.managerPlace = new ManagerPlace(logger, () => this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("SearchAutocomplete")]
        public JsonResult SearchAutocomplete([FromBody] Request<RequestSearchAutocomplete> request)
        {
            if (request.Data == null)
                return Error("Place - GetNext : Data required.");
            return managerPlace.SafeCall(() => managerPlace.SearchAutocomplete(request.Data.Text, request.Data.Quantity));
        }

        [HttpPost]
        [Route("GetPlace")]
        public JsonResult GetPlace([FromBody] Request<RequestGetPlace> request)
        {
            if (request.Data == null)
                return Error("Place - GetPlace : Data required.");
            return managerPlace.SafeCall(() => managerPlace.GetPlace(request.Data.Id));
        }

        [HttpPost]
        [Route("Review")]
        public JsonResult Review([FromBody] Request<RequestReview> request)
        {
            if (request.Data == null)
                return Error("Place - Review : Data required.");
            return managerPlace.SafeCall(() => managerPlace.Review(request.Data.IdPlace, request.Data.Rating, request.Data.Message, request.Data.Flags, request.Data.Pictures));
        }
    }
}
