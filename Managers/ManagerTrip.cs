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
using TripickServer.Models.Common;

namespace TripickServer.Managers
{
    public class ManagerTrip : ManagerBase
    {
        #region Properties

        private RepoTrip repoTrip;
        private RepoPick repoPick;
        private RepoPlaceFlag repoFilter;

        #endregion

        #region Constructor

        public ManagerTrip(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoPick = new RepoPick(this.ConnectedUser, tripickContext);
            this.repoFilter = new RepoPlaceFlag(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            List<Trip> trips = this.repoTrip.GetAll(pageIndex, pageSize).OrderByDescending(x => x.CreationDate).ToList();
            trips.ForEach(trip =>
            {
                trip.NbPicks = this.repoPick.CountNotZeroByTrip(trip.Id);
                trip.Filters = this.repoFilter.GetFilters(trip.Id);
                //trip.Tiles = this.TripickContext.MapTiles.Where(t => t.IdTrip == trip.Id).ToList();
            });
            return trips;
        }

        public Trip GetById(int id)
        {
            Trip trip = this.repoTrip.GetById(id);
            trip.NbPicks = this.repoPick.CountNotZeroByTrip(id);
            return trip;
        }

        public Trip Create(string name, string photo)
        {
            //int numberOfTrips = this.repoTrip.Count() % 5;
            //Configuration configs = repoConfiguration.Get("TripCoverImage" + numberOfTrips);
            // Create
            Trip trip = new Trip()
            {
                IdOwner = this.ConnectedUser().Id,
                CoverImage = photo,
                Name = name,
                Description = string.Empty
            };
            this.repoTrip.Add(trip);

            // Commit
            this.TripickContext.SaveChanges();
            return trip;
        }

        public Trip Update(Trip trip)
        {
            // Verify the entity to update is not null
            if (trip == null)
                throw new NullReferenceException("The trip to update cannot be null");

            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetByIdWithTiles(trip.Id);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={trip.Id}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            //CoverImage = existing.CoverImage;
            existing.Name = trip.Name;
            existing.Description = trip.Description;

            existing.StartDate = trip.StartDate;
            existing.StartLatitude = trip.StartLatitude;
            existing.StartLongitude = trip.StartLongitude;
            existing.StartLatitudeDelta = trip.StartLatitudeDelta;
            existing.StartLongitudeDelta = trip.StartLongitudeDelta;

            existing.EndDate = trip.EndDate;
            existing.EndLatitude = trip.EndLatitude;
            existing.EndLongitude = trip.EndLongitude;
            existing.EndLatitudeDelta = trip.EndLatitudeDelta;
            existing.EndLongitudeDelta = trip.EndLongitudeDelta;

            existing.NbPicks = this.repoPick.CountNotZeroByTrip(trip.Id);

            if(trip.Region != null)
            {
                existing.Region = new Location()
                {
                    IdTrip = existing.Id,
                    Latitude = trip.Region.Latitude,
                    Longitude = trip.Region.Longitude,
                    LatitudeDelta = trip.Region.LatitudeDelta,
                    LongitudeDelta = trip.Region.LongitudeDelta
                };
            }

            if(trip.Polygon != null)
            {
                for (int i = 0; i < trip.Polygon.Count; i++)
                {
                    trip.Polygon[i] = new MapPoint()
                    {
                        Index = i,
                        IdTrip = existing.Id,
                        Latitude = trip.Polygon[i].Latitude,
                        Longitude = trip.Polygon[i].Longitude
                    };
                }
                existing.Polygon = trip.Polygon;
            }

            if (trip.Tiles != null)
            {
                for (int i = 0; i < trip.Tiles.Count; i++)
                {
                    trip.Tiles[i] = new MapTile()
                    {
                        IdTrip = existing.Id,
                        Latitude = trip.Tiles[i].Latitude,
                        Longitude = trip.Tiles[i].Longitude,
                        Height = trip.Tiles[i].Height,
                        Width = trip.Tiles[i].Width
                    };
                }
                existing.Tiles = trip.Tiles;
            }

            // Commit
            this.TripickContext.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetById(id);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={id}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            // Delete
            this.repoTrip.Delete(existing);

            // Commit
            this.TripickContext.SaveChanges();
            return true;
        }

        public string SaveCover(int idTrip, string cover)
        {
            // Verify the entity to update is not null
            if (cover == null)
                throw new NullReferenceException("Cover photo required");

            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetById(idTrip);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={idTrip}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            existing.CoverImage = cover;

            // Commit
            this.TripickContext.SaveChanges();
            return existing.CoverImage;
        }

        public bool SaveFilters(int idTrip, List<PlaceFlag> filters)
        {
            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetById(idTrip);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={idTrip}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            this.repoFilter.Save(idTrip, filters);

            return true;
        }

        #endregion
    }
}
