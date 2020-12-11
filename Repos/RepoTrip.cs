﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoTrip : RepoCRUD<Trip>
    {
        #region Constructor

        public RepoTrip(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            List<Trip> trips = this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(t => t.Region)
                .Include(t => t.Polygon)
                .Include(t => t.Tiles)
                .Include(t => t.Members)
                .Include(t => t.Destinations)
                .ToList();
            trips.ForEach(t => t.Region.Trip = null);
            trips.ForEach(t => t.Polygon.ForEach(p => p.Trip = null));
            trips.ForEach(t => t.Tiles.ForEach(tile => tile.Trip = null));
            return trips;
        }
        public int Count()
        {
            return this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id)
                .Count();
        }

        public override Trip GetById(int id)
        {
            Trip trip = this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id && t.Id == id)
                .Include(t => t.Region)
                .Include(t => t.Polygon)
                .Include(t => t.Tiles)
                .Include(t => t.Members)
                .Include(t => t.Destinations)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.ToBrings)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.Steps)
                .Include(t => t.Picks).ThenInclude(p => p.Place)
                .FirstOrDefault();

            trip.Region.Trip = null;
            trip.Polygon.ForEach(p => p.Trip = null);
            trip.Tiles.ForEach(tile => tile.Trip = null);

            return trip;
        }

        public Trip GetFullById(int id)
        {
            Trip trip = this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id && t.Id == id)
                .Include(t => t.Region)
                .Include(t => t.Polygon)
                .Include(t => t.Tiles)
                .Include(t => t.Members)
                .Include(t => t.Destinations)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.ToBrings)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.Steps)
                .Include(t => t.Picks).ThenInclude(p => p.Place)
                .FirstOrDefault();

            trip.Region.Trip = null;
            trip.Polygon.ForEach(p => p.Trip = null);
            trip.Tiles.ForEach(tile => tile.Trip = null);

            return trip;
        }
    }
}
