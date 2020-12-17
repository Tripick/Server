using Microsoft.EntityFrameworkCore;
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

        public int Count()
        {
            return this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id)
                .Count();
        }

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            List<Trip> trips = this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(t => t.Region)
                .Include(t => t.Polygon)
                .Include(t => t.Destinations)
                .Include(t => t.Members).ThenInclude(m => m.Photo)
                .Include(t => t.Subscribers).ThenInclude(s => s.Photo)
                .ToList();

            trips.ForEach(trip =>
            {
                trip.Polygon = trip.Polygon.OrderBy(p => p.Index).ToList();
                trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
                trip.Members = null;
                trip.Followers = trip.Subscribers == null ? new List<Follower>() : trip.Subscribers.Select(f => new Follower(f)).ToList();
                trip.Subscribers = null;
                trip.Owner = null;
            });
            return trips;
        }

        public override Trip GetById(int id)
        {
            Trip trip = GetPlainById(id);
            trip.Polygon = trip.Polygon.OrderBy(p => p.Index).ToList();
            trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
            trip.Members = null;
            trip.Followers = trip.Subscribers == null ? new List<Follower>() : trip.Subscribers.Select(f => new Follower(f)).ToList();
            trip.Subscribers = null;
            trip.Owner = null;
            return trip;
        }

        public Trip GetPlainById(int id)
        {
            Trip trip = this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id && t.Id == id)
                .Include(t => t.Region)
                .Include(t => t.Polygon)
                .Include(t => t.Destinations)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.ToBrings)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.Steps)
                .Include(t => t.Picks).ThenInclude(p => p.Place)
                .Include(t => t.Members).ThenInclude(m => m.Photo)
                .Include(t => t.Subscribers).ThenInclude(s => s.Photo)
                .FirstOrDefault();
            return trip;
        }

        public Trip GetByIdWithTiles(int id)
        {
            Trip trip = this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser().Id && t.Id == id)
                .Include(t => t.Region)
                .Include(t => t.Polygon)
                .Include(t => t.Destinations)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.ToBrings)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.Steps)
                .Include(t => t.Picks).ThenInclude(p => p.Place)
                .Include(t => t.Members).ThenInclude(m => m.Photo)
                .Include(t => t.Subscribers).ThenInclude(s => s.Photo)
                .FirstOrDefault();

            trip.Polygon = trip.Polygon.OrderBy(p => p.Index).ToList();
            trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
            trip.Members = null;
            trip.Followers = trip.Subscribers == null ? new List<Follower>() : trip.Subscribers.Select(f => new Follower(f)).ToList();
            trip.Subscribers = null;
            trip.Owner = null;

            List<MapTile> tiles = new List<MapTile>();
            if(trip != null)
                this.TripickContext.MapTiles.Where(t => t.IdTrip == trip.Id).ToList();
            trip.Tiles = tiles;

            return trip;
        }
    }
}
