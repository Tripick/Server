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

            if (!trip.Friendships.Any(x => x.Friendship.IdOwner == trip.IdOwner && x.Friendship.IdFriend == idFriend))
                trip.Friendships.Add(new TripFriendship() { IdTrip = idTrip, IdOwner = trip.IdOwner, IdFriend = idFriend });

            this.TripickContext.SaveChanges();
            return trip;
        }

        public Trip Delete(int idTrip, int idFriend)
        {
            Trip trip = repoTrip.GetById(idTrip);

            if (trip.Friendships.Any(x => x.Friendship.IdOwner == trip.IdOwner && x.Friendship.IdFriend == idFriend))
                trip.Friendships.RemoveAt(trip.Friendships.IndexOf(trip.Friendships.First(x => x.Friendship.IdOwner == trip.IdOwner && x.Friendship.IdFriend == idFriend)));

            this.TripickContext.SaveChanges();
            return trip;
        }

        #endregion
    }
}
