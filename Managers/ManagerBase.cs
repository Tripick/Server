using Microsoft.Extensions.Logging;
using System;
using TripickServer.Models;
using TripickServer.Utils;

namespace TripickServer.Managers
{
    public class ManagerBase
    {
        #region Properties

        protected readonly ILogger<ServerLogger> Logger;
        protected readonly Func<AppUser> ConnectedUser;
        protected readonly TripickContext TripickContext;
        

        #endregion

        #region Constructor

        public ManagerBase(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext)
        {
            this.Logger = logger;
            this.ConnectedUser = user;
            this.TripickContext = tripickContext;
        }

        #endregion
    }
}
