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
            Itinerary itinerary = this.TripickContext.Itineraries.Where(i => i.IdTrip == idTrip)
                .Include(i => i.Days)
                .ThenInclude(d => d.Steps)
                .ThenInclude(s => s.Visit)
                .ThenInclude(v => v.Place)
                .FirstOrDefault();
            itinerary.Days = itinerary.Days.OrderBy(d => d.Index).ToList();
            itinerary.Days.ForEach(d => d.Steps = d.Steps.OrderBy(s => s.Index).ToList());
            return itinerary;
        }
    }
}
