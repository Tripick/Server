using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripickServer.Models;
using TripickServer.Utils;
using TripickServer.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace TripickServer.Managers
{
    public class ManagerPick : ManagerBase
    {
        #region Properties

        private RepoTrip repoTrip;
        private RepoPlace repoPlace;
        private RepoPick repoPick;

        #endregion

        #region Constructor

        public ManagerPick(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoPlace = new RepoPlace(this.ConnectedUser, tripickContext);
            this.repoPick = new RepoPick(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public bool Generate(int idTrip)
        {
            Trip trip = this.repoTrip.GetByIdWithTiles(idTrip);

            List<BoundingBox> areas = trip.Tiles.Select(t => new BoundingBox()
            {
                MinLat = t.Latitude,
                MinLon = t.Longitude,
                MaxLat = t.Latitude + t.Width,
                MaxLon = t.Longitude + t.Height
            }).ToList();

            // Get places respecting area
            List<Place> places = this.repoPlace.GetAllInAreas(areas);

            // Ordering places by preferences
            var query = places.Where(p => p.PriceLevel == null || (int.Parse(p.PriceLevel) + 1) < trip.FilterExpensive);

            if (trip.FilterSportive == 1) query = query.Where(p =>
                !p.Types.Contains("park") &&
                !p.NameTranslated.Contains("park") &&
                !p.Types.Contains("place_of_worship")
            );
            else if (trip.FilterSportive == 2) query = query.Where(p =>
                !p.Types.Contains("park") &&
                !p.NameTranslated.Contains("park")
            );
            else if (trip.FilterSportive == 4) query = query.Where(p =>
                !p.Types.Contains("museum") &&
                !p.NameTranslated.Contains("museum") &&
                !p.Types.Contains("spa")
            );
            else if (trip.FilterSportive == 5) query = query.Where(p =>
                !p.Types.Contains("museum") &&
                !p.NameTranslated.Contains("museum") &&
                !p.Types.Contains("place_of_worship") &&
                !p.Types.Contains("spa")
            );

            if (trip.FilterFamous == 3) query = query.OrderBy(p => p.Rating);
            else if (trip.FilterFamous == 1) query = query.OrderBy(p => p.NbRating).ThenBy(p => p.Rating);
            else if (trip.FilterFamous == 2) query = query.Where(p => p.NbRating > 0 && p.NbRating < 500).OrderByDescending(p => p.NbRating).ThenBy(p => p.Rating);
            else if (trip.FilterFamous == 4) query = query.Where(p => p.NbRating > 500).OrderBy(p => p.NbRating).ThenBy(p => p.Rating);
            else if (trip.FilterFamous == 5) query = query.OrderByDescending(p => p.NbRating).ThenBy(p => p.Rating);

            places = query.ToList();

            // Generate picks
            List<Pick> picks = new List<Pick>();
            for (int i = 0; i < places.Count; i++)
            {
                picks.Add(new Pick() { Index = i, IdPlace = places[i].Id, IdTrip = idTrip, IdUser = this.ConnectedUser().Id, Rating = -1 });
            }
            //for (int i = 0; i < picks.Count; i++)
            //{
            //    this.repoPick.Add(picks[i]);
            //}

            // Commit
            //this.TripickContext.SaveChanges();
            return true;
        }

        #endregion
    }
}
