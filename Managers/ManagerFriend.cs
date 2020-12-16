using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripickServer.Models;
using TripickServer.Utils;
using TripickServer.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TripickServer.Managers
{
    public class ManagerFriend : ManagerBase
    {
        #region Properties

        private UserManager<AppUser> userManager;
        private TripickContext tripickContext;

        #endregion

        #region Constructor

        public ManagerFriend(ILogger<ServerLogger> logger, Func<AppUser> user, UserManager<AppUser> userManager, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.userManager = userManager;
            this.tripickContext = tripickContext;
        }

        #endregion

        #region Private

        public List<Friend> Add(int id)
        {
            AppUser newFriend = this.tripickContext.Users.Where(x => x.Id == id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (newFriend == null)
                throw new NullReferenceException($"The user [Id={id}] does not exist");

            AppUser user = this.tripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            if(!user.Friendships.Any(x => x.IdFriend == newFriend.Id))
                user.Friendships.Add(new Friendship() { IdOwner = user.Id, IdFriend = newFriend.Id });

            if (!newFriend.Friendships.Any(x => x.IdFriend == user.Id)) 
                newFriend.Friendships.Add(new Friendship() { IdOwner = newFriend.Id, IdFriend = user.Id });

            this.tripickContext.SaveChanges();

            List<Friend> friends = new List<Friend>();
            if (user != null && user.Friendships != null)
            {
                List<int> ids = user.Friendships.Select(x => x.IdFriend).ToList();
                List<AppUser> friendsUsers = this.userManager.Users.Where(x => ids.Contains(x.Id)).ToList();
                friends = friendsUsers.Select(x => new Friend(x)).ToList();
            }
            return friends;
        }

        public List<Friend> Delete(int id)
        {
            AppUser newFriend = this.tripickContext.Users.Where(x => x.Id == id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (newFriend == null)
                throw new NullReferenceException($"The user [Id={id}] does not exist");

            AppUser user = this.tripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            if (user.Friendships.Any(x => x.IdFriend == newFriend.Id))
                user.Friendships.RemoveAt(user.Friendships.IndexOf(user.Friendships.First(x => x.IdFriend == newFriend.Id)));

            if (newFriend.Friendships.Any(x => x.IdFriend == user.Id))
                newFriend.Friendships.RemoveAt(newFriend.Friendships.IndexOf(newFriend.Friendships.First(x => x.IdFriend == user.Id)));

            this.tripickContext.SaveChanges();

            List<Friend> friends = new List<Friend>();
            if (user != null && user.Friendships != null)
            {
                List<int> ids = user.Friendships.Select(x => x.IdFriend).ToList();
                List<AppUser> friendsUsers = this.userManager.Users.Where(x => ids.Contains(x.Id)).ToList();
                friends = friendsUsers.Select(x => new Friend(x)).ToList();
            }
            return friends;
        }

        #endregion
    }
}
