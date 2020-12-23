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
        [Route("Generate")]
        public JsonResult Generate([FromBody] Request<RequestGenerate> request)
        {
            if (request.Data == null)
                return Error("Pick - Generate : Data required.");
            return managerPick.SafeCall(() => managerPick.Generate(request.Data.IdTrip));
        }
    }
}
