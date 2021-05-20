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
        private RepoFilter repoFilter;

        #endregion

        #region Constructor

        public ManagerPick(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoPlace = new RepoPlace(this.ConnectedUser, tripickContext);
            this.repoPick = new RepoPick(this.ConnectedUser, tripickContext);
            this.repoFilter = new RepoFilter(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public AllPicks GetAll(int idTrip, int quantity, int skip)
        {
            List<Pick> picks = this.repoPick.GetAll(idTrip, quantity, skip);
            picks.ForEach(p =>
            {
                p.User = null;
            });
            return new AllPicks()
            {
                ExistingPicksCount = this.repoPick.CountAllByTrip(idTrip),
                Picks = picks
            };
        }

        public NextPicks GetNexts(int idTrip, int quantity, List<int> idsToExclude = null)
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
            int existingPicksCount = existingPicksIds.Count;

            // Exclude ids to exclude
            if(idsToExclude != null && idsToExclude.Any())
                existingPicksIds.AddRange(idsToExclude);

            // Get filters of the connected user for the trip
            List<Filter> filters = this.repoFilter.GetAllForTrip(idTrip);

            // Get places respecting area
            List<Place> places = this.repoPlace.GetNextsToPick(existingPicksIds, areas, filters, quantity);
            int count = this.repoPlace.CountNextsToPick(existingPicksIds, areas, filters);
            // If the number of places fetched is lower than the quantity needed : include also the places who don't have filter information (Price, Length, ...)
            if (places.Count < quantity)
            {
                places.AddRange(this.repoPlace.GetNextsToPick(existingPicksIds, areas, null, quantity - places.Count));
                count += this.repoPlace.CountNextsToPick(existingPicksIds, areas, null);
            }

            // Generate picks to be rated
            List<Pick> picks = new List<Pick>();
            for (int i = 0; i < places.Count; i++)
            {
                places[i].Images.ForEach(i => { i.Place = null; i.Uploader = null; });
                places[i].Flags.ForEach(f => f.Place = null);
                places[i].Reviews.ForEach(r =>
                {
                    r.Place = null;
                    r.Author = new AppUser() { UserName = r.Author.UserName, Photo = r.Author.Photo };
                    r.Flags.ForEach(f => f.Review = null);
                    r.Pictures.ForEach(p => p.Review = null);
                });
                picks.Add(new Pick() { Index = i, IdPlace = places[i].Id, IdTrip = idTrip, IdUser = this.ConnectedUser().Id, Rating = -1, Place = places[i] });
            }

            return new NextPicks() { Count = count, ExistingPicksCount = existingPicksCount, Picks = picks };
        }

        public NextPicks SavePick(int idTrip, int idPlace, int rating, List<int> alreadyLoaded)
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

            // Send next pick
            return GetNexts(idTrip, 1, alreadyLoaded);
        }

        #endregion
    }
}
