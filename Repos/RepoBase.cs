using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoBase
    {
        #region Properties

        protected readonly AppUser ConnectedUser;
        protected readonly TripickContext TripickContext;

        #endregion

        #region Constructor

        public RepoBase(AppUser connectedUser, TripickContext tripickContext)
        {
            this.ConnectedUser = connectedUser;
            this.TripickContext = tripickContext;
        }

        #endregion
    }
}
