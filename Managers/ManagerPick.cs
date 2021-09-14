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

        public List<Place> SearchMap(int idTrip, Region region)
        {
            Trip trip = this.repoTrip.GetByIdWithTiles(idTrip);
            List<BoundingBox> tripAreas = trip.Tiles.Select(t => new BoundingBox()
            {
                MinLat = t.Latitude,
                MinLon = t.Longitude,
                MaxLat = t.Latitude + t.Height,
                MaxLon = t.Longitude + t.Width
            }).ToList();
            BoundingBox area = new BoundingBox()
            {
                MinLat = region.Latitude - region.LatitudeDelta / 2,
                MinLon = region.Longitude - region.LongitudeDelta / 2,
                MaxLat = region.Latitude + region.LatitudeDelta / 2,
                MaxLon = region.Longitude + region.LongitudeDelta / 2
            };
            // Only show places who are in the area to search AND in the tiles of the trip
            List<BoundingBox> areas = new List<BoundingBox>();
            foreach (BoundingBox bb in tripAreas)
            {
                if (bb.MinLat < area.MinLat && bb.MinLon < area.MinLon && bb.MaxLat > area.MaxLat && bb.MaxLon > area.MaxLon) //area is Contained
                {
                    areas = new List<BoundingBox>();
                    areas.Add(new BoundingBox()
                    {
                        MinLat = area.MinLat,
                        MinLon = area.MinLon,
                        MaxLat = area.MaxLat,
                        MaxLon = area.MaxLon
                    });
                    break;
                }
                else if(area.MinLat < bb.MinLat && area.MinLon < bb.MinLon && area.MaxLat > bb.MaxLat && area.MaxLon > bb.MaxLon) //area is Containing
                {
                    areas.Add(new BoundingBox()
                    {
                        MinLat = bb.MinLat,
                        MinLon = bb.MinLon,
                        MaxLat = bb.MaxLat,
                        MaxLon = bb.MaxLon
                    });
                }
                else if (!(area.MinLat > bb.MaxLat || area.MaxLat < bb.MinLat || area.MinLon > bb.MaxLon || area.MaxLon < bb.MinLon)) //area is Crossing
                {
                    areas.Add(new BoundingBox()
                    {
                        MinLat = Math.Max(area.MinLat, bb.MinLat),
                        MinLon = Math.Max(area.MinLon, bb.MinLon),
                        MaxLat = Math.Min(area.MaxLat, bb.MaxLat),
                        MaxLon = Math.Min(area.MaxLon, bb.MaxLon)
                    });
                }
            }

            // Get existing picks for this user and trip
            List<int> existingPicksIds = this.repoPick.GetAllByTrip(idTrip).Select(x => x.IdPlace).ToList();

            // Get places respecting area
            List<Place> places = this.repoPlace.GetNextsToPick(existingPicksIds, areas, null, 10);
            return places;
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
