using System;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoItineraryDayStep : RepoCRUD<ItineraryDayStep>
    {
        #region Constructor

        public RepoItineraryDayStep(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

    }
}
