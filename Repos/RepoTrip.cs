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
                .Include(t => t.Members)
                .Include(t => t.Subscribers)
                .ToList();

            foreach (Trip trip in trips)
            {
                trip.Region = this.TripickContext.Locations.FirstOrDefault(l => l.IdTrip == trip.Id);
            }
            foreach (Trip trip in trips)
            {
                List<MapPoint> polygon = new List<MapPoint>();
                if (trip != null)
                    polygon = this.TripickContext.MapPoint.Where(p => p.IdTrip == trip.Id).ToList();
                trip.Polygon = polygon.OrderBy(p => p.Index).ToList();
            }

            return trips;
        }

        public override Trip GetById(int id)
        {
            Trip trip = this.TripickContext.Trips
                .Where(t => t.Id == id)
                .Include(t => t.Members)
                .Include(t => t.Subscribers)
                .FirstOrDefault();
            if (trip.IsDeleted || trip.IdOwner != this.ConnectedUser().Id)
                return null;

            // Region
            trip.Region = this.TripickContext.Locations.FirstOrDefault(l => l.IdTrip == trip.Id);

            // Polygon
            List<MapPoint> polygon = new List<MapPoint>();
            if (trip != null)
                polygon = this.TripickContext.MapPoint.Where(p => p.IdTrip == trip.Id).ToList();
            trip.Polygon = polygon.OrderBy(p => p.Index).ToList();
            return trip;
        }

        public Trip GetByIdWithTiles(int id)
        {
            Trip trip = GetById(id);

            // Tiles
            List<MapTile> tiles = new List<MapTile>();
            if(trip != null)
                tiles = this.TripickContext.MapTiles.Where(t => t.IdTrip == trip.Id).ToList();
            trip.Tiles = tiles;

            return trip;
        }
    }
}
