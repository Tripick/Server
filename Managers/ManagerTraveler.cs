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
    public class ManagerTraveler : ManagerBase
    {
        #region Properties

        private RepoTrip repoTrip;

        #endregion

        #region Constructor

        public ManagerTraveler(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public bool Save(int idTrip, List<int> idsFriends)
        {
            Trip trip = repoTrip.GetById(idTrip);
            AppUser user = this.TripickContext.Users.Include(x => x.Friendships).Where(x => x.Id == this.ConnectedUser().Id).SingleOrDefault();
            if (idsFriends.Any(f => !user.Friendships.Any(x => x.IdFriend == f)))
                throw new NullReferenceException("Impossible to add a traveler to the trip if he is not your friend.");
            List<AppUser> friends = this.TripickContext.Users.Where(x => idsFriends.Contains(x.Id)).Include(x => x.Photo).ToList();
            trip.Members.Clear();
            trip.Members.AddRange(friends);
            this.TripickContext.SaveChanges();
            return true;
        }

        public bool Delete(int idTrip, int idFriend)
        {
            Trip trip = repoTrip.GetById(idTrip);

            if (trip.Members.Any(x => x.Id == idFriend))
                trip.Members.RemoveAt(trip.Members.IndexOf(trip.Members.First(x => x.Id == idFriend)));

            this.TripickContext.SaveChanges();

            return true;
        }

        #endregion
    }
}
