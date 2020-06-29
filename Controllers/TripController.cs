using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TripickServer.Managers;
using TripickServer.Models;
using TripickServer.Utils;

namespace TripickServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class TripController : BaseController
    {
        #region Properties

        private readonly ManagerTrip managerTrip;

        #endregion

        #region Constructor

        public TripController(IHttpContextAccessor contextAccessor, ILogger<ServerLogger> logger, TripickContext tripickContext) : base(contextAccessor, logger)
        {
            this.managerTrip = new ManagerTrip(logger, this.ConnectedUser, tripickContext);
        }

        #endregion

        [HttpPost]
        [Route("Create")]
        public ServerResponse<Trip> Create(Trip trip)
        {
            return this.managerTrip.Create(trip);
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
