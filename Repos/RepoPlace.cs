using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoPlace : RepoCRUD<Place>
    {
        #region Constructor

        public RepoPlace(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<Place> GetAllInAreas(List<BoundingBox> areas)
        {
            List<Place> places = this.TripickContext.Places
                .Where(p => areas.Any(a => 
                    a.MinLat < p.Latitude &&
                    a.MaxLat > p.Latitude &&
                    a.MinLat < p.Longitude &&
                    a.MaxLat > p.Longitude))
                .ToList();
            return places;
        }
    }
}
