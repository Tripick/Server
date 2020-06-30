﻿using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoTrip : RepoCRUD<Trip>
    {
        #region Constructor

        public RepoTrip(AppUser connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            return this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser.Id)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Include(t => t.Members)
                .Include(t => t.Destinations)
                .ToList();
        }

        public Trip GetFullById(int id)
        {
            return this.TripickContext.Trips
                .Where(t => !t.IsDeleted && t.IdOwner == this.ConnectedUser.Id && t.Id == id)
                .Include(t => t.Members)
                .Include(t => t.Destinations)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.ToBrings)
                .Include(t => t.Itinerary).ThenInclude(i => i.Days).ThenInclude(d => d.Steps)
                .Include(t => t.Picks).ThenInclude(p => p.Place)
                .FirstOrDefault();
        }
    }
}