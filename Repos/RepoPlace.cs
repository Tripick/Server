using LinqKit;
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
            var query = PredicateBuilder.New<Place>();
            foreach (BoundingBox area in areas)
            {
                query = query.Or(p =>
                    area.MinLat < p.Latitude &&
                    area.MaxLat > p.Latitude &&
                    area.MinLon < p.Longitude &&
                    area.MaxLon > p.Longitude);
            }
            List<Place> places = this.TripickContext.Places.AsExpandable().Where(query).ToList();
            return places;
        }
    }
}
