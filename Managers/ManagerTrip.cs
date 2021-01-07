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

        #region Private

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            List<Trip> trips = this.repoTrip.GetAll(pageIndex, pageSize).OrderByDescending(x => x.CreationDate).ToList();
            trips.ForEach(trip =>
            {
                trip.Polygon = trip.Polygon.OrderBy(p => p.Index).ToList();
                trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
                trip.Members = null;
                trip.Followers = trip.Subscribers == null ? new List<Follower>() : trip.Subscribers.Select(f => new Follower(f)).ToList();
                trip.Subscribers = null;
            });
            return trips;
        }

        public Trip GetById(int id)
        {
            Trip trip = this.repoTrip.GetById(id);
            trip.Owner = null;
            trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
            trip.Members = null;
            trip.Followers = trip.Subscribers == null ? new List<Follower>() : trip.Subscribers.Select(f => new Follower(f)).ToList();
            trip.Subscribers = null;
            return trip;
        }

        public Trip Create()
        {
            int numberOfTrips = this.repoTrip.Count() % 5;
            Configuration configs = repoConfiguration.Get("TripCoverImage" + numberOfTrips);
            // Create
            Trip trip = new Trip()
            {
                IdOwner = this.ConnectedUser().Id,
                IsPublic = false,
                CoverImage = configs?.Value,
                Name = "My new trip",
                Description = string.Empty,
                Note = string.Empty
            };
            this.repoTrip.Add(trip);

            // Commit
            this.TripickContext.SaveChanges();

            trip.Owner = null;
            trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
            trip.Members = null;
            trip.Followers = trip.Subscribers == null ? new List<Follower>() : trip.Subscribers.Select(f => new Follower(f)).ToList();
            trip.Subscribers = null;
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

            if(trip.Tiles != null && trip.Tiles.Any())
                existing.Tiles = trip.Tiles;

            // Update
            //this.repoTrip.Update(existing);

            // Commit
            this.TripickContext.SaveChanges();

            // Do not send unuseful data
            existing.Owner = null;
            existing.CoverImage = null;
            existing.Tiles = null;
            existing.Travelers = existing.Members == null ? new List<Traveler>() : existing.Members.Select(f => new Traveler(f)).ToList();
            existing.Members = null;
            existing.Followers = existing.Subscribers == null ? new List<Follower>() : existing.Subscribers.Select(f => new Follower(f)).ToList();
            existing.Subscribers = null;
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
