using System;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoBase
    {
        #region Properties

        protected readonly Func<AppUser> ConnectedUser;
        protected readonly TripickContext TripickContext;

        #endregion

        #region Constructor

        public RepoBase(Func<AppUser> connectedUser, TripickContext tripickContext)
        {
            this.ConnectedUser = connectedUser;
            this.TripickContext = tripickContext;
        }

        #endregion
    }
}
