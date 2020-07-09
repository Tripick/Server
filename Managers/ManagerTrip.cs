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
            return this.repoTrip.GetAll(pageIndex, pageSize);
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
                Note = string.Empty,
                StartDate = DateTime.Today.AddDays(1),
                StartLatitude = null,
                StartLongitude = null,
                EndDate = DateTime.Today.AddDays(7),
                EndLatitude = null,
                EndLongitude = null
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
                throw new NullReferenceException("The trip to create cannot be null");

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
            existing.DateMode = trip.DateMode;
            existing.StartDate = trip.StartDate;
            existing.StartLatitude = trip.StartLatitude;
            existing.StartLongitude = trip.StartLongitude;
            existing.EndDate = trip.EndDate;
            existing.EndLatitude = trip.EndLatitude;
            existing.EndLongitude = trip.EndLongitude;
            existing.FilterIntense = trip.FilterIntense;
            existing.FilterSportive = trip.FilterSportive;
            existing.FilterCity = trip.FilterCity;
            existing.FilterFamous = trip.FilterFamous;
            existing.FilterFar = trip.FilterFar;
            existing.FilterExpensive = trip.FilterExpensive;

            // Update
            //this.repoTrip.Update(existing);

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

        #endregion
    }
}
