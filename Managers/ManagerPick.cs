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
        private RepoPlaceFlag repoFilter;

        #endregion

        #region Constructor

        public ManagerPick(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoPlace = new RepoPlace(this.ConnectedUser, tripickContext);
            this.repoPick = new RepoPick(this.ConnectedUser, tripickContext);
            this.repoFilter = new RepoPlaceFlag(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public AllPicks GetAll(int idTrip, int quantity, int skip)
        {
            List<Pick> picks = this.repoPick.GetAll(idTrip, quantity, skip);
            return new AllPicks()
            {
                ExistingPicksCount = this.repoPick.CountAllByTrip(idTrip),
                Picks = picks
            };
        }

        public NextPicks GetNexts(int idTrip, int quantity, List<int> alreadyLoaded = null)
        {
            Trip trip = this.repoTrip.GetByIdWithTiles(idTrip);
            List<BoundingBox> areas = trip.Tiles.Select(t => new BoundingBox()
            {
                MinLat = t.Latitude,
                MinLon = t.Longitude,
                MaxLat = t.Latitude + t.Height,
                MaxLon = t.Longitude + t.Width
            }).ToList();

            // Get existing picks for this user and trip
            List<Pick> existingPicks = this.repoPick.GetAllByTrip(idTrip);
            List<int> existingPicksIds = existingPicks.Select(x => x.IdPlace).ToList();
            int existingPicksCount = existingPicks.Count(p => p.Rating > 0);

            // Exclude ids to exclude
            if(alreadyLoaded != null && alreadyLoaded.Any())
                existingPicksIds.AddRange(alreadyLoaded);

            // Get filters of the connected user for the trip
            List<PlaceFlag> filters = this.repoFilter.GetFilters(idTrip);

            // Get places respecting area
            List<Place> places = this.repoPlace.GetNextsToPick(existingPicksIds, areas, filters, quantity);
            int count = this.repoPlace.CountNextsToPick(existingPicksIds, areas, filters);
            // If the number of places fetched is lower than the quantity needed : include also the places who don't have filter information (Price, Length, ...)
            if (places.Count < quantity)
            {
                List<int> notToPick = new List<int>();
                notToPick.AddRange(existingPicksIds);
                notToPick.AddRange(places.Select(p => p.Id).ToList());
                places.AddRange(this.repoPlace.GetNextsToPick(notToPick, areas, null, quantity - places.Count));
                count += this.repoPlace.CountNextsToPick(existingPicksIds, areas, null);
            }

            // Generate picks to be rated
            List<Pick> picks = new List<Pick>();
            for (int i = 0; i < places.Count; i++)
            {
                picks.Add(new Pick() { Index = i, IdPlace = places[i].Id, IdTrip = idTrip, IdUser = this.ConnectedUser().Id, Rating = -1, Place = places[i] });
            }

            return new NextPicks() { Count = count, ExistingPicksCount = existingPicksCount, Picks = picks };
        }

        public Pick GetSingle(int id)
        {
            Place place = this.repoPlace.GetComplete(id);
            return new Pick() { Index = 0, IdPlace = id, IdTrip = 0, IdUser = this.ConnectedUser().Id, Rating = -1, Place = place };
        }

        public int SavePick(int idTrip, int idPlace, int rating)
        {
            // Save pick
            Pick pick = new Pick() { IdPlace = idPlace, IdUser = this.ConnectedUser().Id, IdTrip = idTrip, Rating = rating };

            Pick existingPick = this.repoPick.Find(idTrip, idPlace);
            if(existingPick != null)
                existingPick.Rating = pick.Rating;
            else
                this.repoPick.Add(pick);

            // Commit
            this.TripickContext.SaveChanges();

            // Send pick count
            return this.repoPick.CountNotZeroByTrip(idTrip);
        }

        #endregion
    }
}
