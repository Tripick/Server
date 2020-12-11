﻿using Microsoft.Extensions.Logging;
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
    public class ManagerTrip : ManagerBase
    {
        #region Properties

        private RepoConfiguration repoConfiguration;
        private RepoTrip repoTrip;

        #endregion

        #region Constructor

        public ManagerTrip(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoConfiguration = new RepoConfiguration(this.ConnectedUser, tripickContext);
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public JsonResult SafeCall<T>(Func<T> method)
        {
            try
            {
                return ServerResponse<T>.ToJson(method());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        #endregion

        #region Private

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            return this.repoTrip.GetAll(pageIndex, pageSize).OrderByDescending(x => x.CreationDate).ToList();
        }

        public Trip GetById(int id, bool full = false)
        {
            // Get the entity
            Trip trip = full ? this.repoTrip.GetFullById(id) : this.repoTrip.GetById(id);

            // Verify the entity is mine
            if (trip.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");
            else
                return trip;
        }

        public Trip Create()
        {
            int numberOfTrips = this.repoTrip.Count() % 5;
            Configuration configs = repoConfiguration.Get("TripCoverImage" + numberOfTrips);
            // Create
            Trip tripToSave = new Trip()
            {
                IdOwner = this.ConnectedUser().Id,
                IsPublic = false,
                CoverImage = configs?.Value,
                Name = "My new trip",
                Description = string.Empty,
                Note = string.Empty
            };
            this.repoTrip.Add(tripToSave);

            // Commit
            this.TripickContext.SaveChanges();
            return tripToSave;
        }

        public Trip Update(Trip trip)
        {
            // Verify the entity to update is not null
            if (trip == null)
                throw new NullReferenceException("The trip to update cannot be null");

            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetFullById(trip.Id);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={trip.Id}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            //CoverImage = existing.CoverImage;
            existing.IsPublic = trip.IsPublic;
            existing.Name = trip.Name;
            existing.Description = trip.Description;
            existing.Note = trip.Note;
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
            existing.FilterIntense = trip.FilterIntense;
            existing.FilterSportive = trip.FilterSportive;
            existing.FilterCity = trip.FilterCity;
            existing.FilterFamous = trip.FilterFamous;
            existing.FilterFar = trip.FilterFar;
            existing.FilterExpensive = trip.FilterExpensive;
            if (existing.Region != null)
            {
                existing.Region.Latitude = trip.Region.Latitude;
                existing.Region.Longitude = trip.Region.Longitude;
                existing.Region.LatitudeDelta = trip.Region.LatitudeDelta;
                existing.Region.LongitudeDelta = trip.Region.LongitudeDelta;
            }
            else
                existing.Region = trip.Region;
            if(trip.Polygon != null)
            {
                for (int i = 0; i < trip.Polygon.Count; i++)
                {
                    trip.Polygon[i].Index = i;
                }
            }
            existing.Polygon = trip.Polygon;
            existing.Tiles = trip.Tiles;

            // Update
            //this.repoTrip.Update(existing);

            // Commit
            this.TripickContext.SaveChanges();

            // Do not send the image again
            existing.CoverImage = null;
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

        #endregion
    }
}
