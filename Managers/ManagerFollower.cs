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
using Microsoft.EntityFrameworkCore;

namespace TripickServer.Managers
{
    public class ManagerFollower : ManagerBase
    {
        #region Properties

        private RepoTrip repoTrip;

        #endregion

        #region Constructor

        public ManagerFollower(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public Follower Add(int idTrip, int idFriend)
        {
            Trip trip = repoTrip.GetById(idTrip);

            AppUser user = this.TripickContext.Users.Include(x => x.Friendships).Where(x => x.Id == this.ConnectedUser().Id).SingleOrDefault();
            if (!user.Friendships.Any(x => x.IdFriend == idFriend))
                throw new NullReferenceException("Impossible to add the follower to the trip if he is not your friend.");

            AppUser friend = this.TripickContext.Users.Where(x => x.Id == idFriend).Include(x => x.Photo).SingleOrDefault();
            trip.Subscribers.Add(friend);
            this.TripickContext.SaveChanges();

            return new Follower(friend);
        }

        public bool Delete(int idTrip, int idFriend)
        {
            Trip trip = repoTrip.GetById(idTrip);

            if (trip.Subscribers.Any(x => x.Id == idFriend))
                trip.Subscribers.RemoveAt(trip.Subscribers.IndexOf(trip.Subscribers.First(x => x.Id == idFriend)));

            this.TripickContext.SaveChanges();

            return true;
        }

        #endregion
    }
}
