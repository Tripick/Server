using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripickServer.Models;
using TripickServer.Utils;
using TripickServer.Repos;
using Microsoft.AspNetCore.Mvc;

namespace TripickServer.Managers
{
    public class ManagerTrip : ManagerBase
    {
        #region Properties

        private RepoTrip repoTrip;

        #endregion

        #region Constructor

        public ManagerTrip(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
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

        public Trip Create(Trip trip)
        {
            // Verify the entity to create is not null
            if (trip == null)
                throw new NullReferenceException("The trip to create cannot be null");

            // Create
            Trip tripToSave = new Trip()
            {
                IdOwner = this.ConnectedUser().Id,
                IsPublic = trip.IsPublic,
                CoverImage = trip.CoverImage,
                Name = trip.Name,
                Description = trip.Description,
                Note = trip.Note,
                StartDate = trip.StartDate,
                EndDate = trip.EndDate,
                StartLatitude = trip.StartLatitude,
                StartLongitude = trip.StartLongitude,
                EndLatitude = trip.EndLatitude,
                EndLongitude = trip.EndLongitude
            };
            this.repoTrip.Add(tripToSave);

            // Commit
            this.TripickContext.SaveChanges();
            return trip;
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

            // Update
            this.repoTrip.Update(trip);

            // Commit
            this.TripickContext.SaveChanges();
            return trip;
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
