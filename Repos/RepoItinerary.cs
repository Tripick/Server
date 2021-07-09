using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoItinerary : RepoCRUD<Itinerary>
    {
        #region Constructor

        public RepoItinerary(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public Itinerary GetByIdTrip(int idTrip)
        {
            Itinerary itinerary = this.TripickContext.Itineraries.Where(i => i.IdTrip == idTrip).FirstOrDefault();
            return itinerary;
        }
    }
}
