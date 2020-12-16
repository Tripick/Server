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
using Microsoft.EntityFrameworkCore;

namespace TripickServer.Managers
{
    public class ManagerTraveler : ManagerBase
    {
        #region Properties

        private RepoConfiguration repoConfiguration;
        private RepoTrip repoTrip;

        #endregion

        #region Constructor

        public ManagerTraveler(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoConfiguration = new RepoConfiguration(this.ConnectedUser, tripickContext);
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public Trip Add(int idTrip, int idFriend)
        {
            Trip trip = repoTrip.GetById(idTrip);

            AppUser user = this.TripickContext.Users.Include(x => x.Friendships).Where(x => x.Id == this.ConnectedUser().Id).SingleOrDefault();
            if (!user.Friendships.Any(x => x.IdFriend == idFriend))
                throw new NullReferenceException("Impossible to add the traveler to the trip if he is not your friend.");

            AppUser friend = this.TripickContext.Users.Where(x => x.Id == idFriend).Include(x => x.Photo).SingleOrDefault();
            trip.Members.Add(friend);
            this.TripickContext.SaveChanges();

            trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
            trip.Members = null;
            return trip;
        }

        public Trip Delete(int idTrip, int idFriend)
        {
            Trip trip = repoTrip.GetById(idTrip);

            if (trip.Members.Any(x => x.Id == idFriend))
                trip.Members.RemoveAt(trip.Members.IndexOf(trip.Members.First(x => x.Id == idFriend)));

            this.TripickContext.SaveChanges();

            trip.Travelers = trip.Members == null ? new List<Traveler>() : trip.Members.Select(f => new Traveler(f)).ToList();
            trip.Members = null;
            return trip;
        }

        #endregion
    }
}
