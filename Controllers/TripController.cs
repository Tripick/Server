using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Requests;
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
            this.managerTrip = new ManagerTrip(logger, this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Create")]
        public JsonResult Create([FromBody] Request<Trip> request)
        {
            if (request.Data == null)
                return Error("The trip to create is null.");
            return managerTrip.SafeCall(() => managerTrip.Create(request.Data));
        }

        [HttpGet]
        [Route("GetAll")]
        public ServerResponse<List<Trip>> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            return this.managerTrip.GetAll(pageIndex, pageSize);
        }

        [HttpGet]
        [Route("Get")]
        public ServerResponse<Trip> Get(int id)
        {
            return this.managerTrip.GetById(id, false);
        }

        [HttpGet]
        [Route("GetFull")]
        public ServerResponse<Trip> GetFull(int id)
        {
            return this.managerTrip.GetById(id, true);
        }

        [HttpPost]
        [Route("Update")]
        public ServerResponse<Trip> Update(Trip trip)
        {
            return this.managerTrip.Update(trip);
        }

        [HttpDelete]
        [Route("Delete")]
        public ServerResponse<bool> Delete(int id)
        {
            return this.managerTrip.Delete(id);
        }
    }
}
