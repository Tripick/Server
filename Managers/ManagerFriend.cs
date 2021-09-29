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

        #endregion

        #region Constructor

        public ManagerFriend(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {}

        #endregion

        #region Private

        public Friend Add(int id)
        {
            AppUser user = this.TripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            AppUser newFriend = this.TripickContext.Users.Where(x => x.Id == id).Include(t => t.Friendships).SingleOrDefault();
            if (newFriend == null)
                throw new NullReferenceException($"The user [Id={id}] does not exist");

            Friendship meToFriend = user.Friendships.FirstOrDefault(x => x.IdFriend == newFriend.Id);
            Friendship friendToMe = newFriend.Friendships.FirstOrDefault(x => x.IdFriend == user.Id);

            // Id there's no invite : create it for myself
            if (meToFriend == null)
            {
                meToFriend = new Friendship() { IdOwner = user.Id, IdFriend = newFriend.Id, NeedToConfirm = newFriend.Id };
                user.Friendships.Add(meToFriend);
            }
            // If there is an invite AND I'm not the one who needs to confirm it : impossible to send an invite twice
            else if (meToFriend.NeedToConfirm == newFriend.Id)
                throw new InvalidOperationException($"The user [Id={id}] already got the friend invite");

            // If the friend didn't receive an invite yet : send it
            if (friendToMe == null)
            {
                friendToMe = new Friendship() { IdOwner = newFriend.Id, IdFriend = user.Id, NeedToConfirm = newFriend.Id };
                newFriend.Friendships.Add(friendToMe);
            }
            // If the friend already sent an invite to me : confirm both
            else if (friendToMe.NeedToConfirm == user.Id)
            {
                meToFriend.NeedToConfirm = null;
                friendToMe.NeedToConfirm = null;
            }

            this.TripickContext.SaveChanges();
            Friend returnedFriend = new Friend(this.TripickContext.Users.Where(x => x.Id == newFriend.Id).Include(t => t.Photo).FirstOrDefault(), meToFriend);
            return returnedFriend;
        }

        public Friend Confirm(int id)
        {
            AppUser user = this.TripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            AppUser inviter = this.TripickContext.Users.Where(x => x.Id == id).Include(t => t.Friendships).SingleOrDefault();
            if (inviter == null)
                throw new NullReferenceException($"The user [Id={id}] does not exist");

            Friendship meToFriend = user.Friendships.FirstOrDefault(x => x.IdFriend == inviter.Id);
            Friendship friendToMe = inviter.Friendships.FirstOrDefault(x => x.IdFriend == user.Id);

            // If the invite exists AND needs to be confirmed by me on both sides : confirm it
            if (meToFriend != null && meToFriend.NeedToConfirm == user.Id && friendToMe != null && friendToMe.NeedToConfirm == user.Id)
            {
                user.Friendships.First(x => x.IdFriend == inviter.Id).NeedToConfirm = null;
                inviter.Friendships.First(x => x.IdFriend == user.Id).NeedToConfirm = null;
            }
            else
                throw new NullReferenceException($"The friend invite of the user [Id={id}] does not exist");

            this.TripickContext.SaveChanges();
            Friend returnedFriend = new Friend(this.TripickContext.Users.Where(x => x.Id == id).Include(t => t.Photo).FirstOrDefault());
            return returnedFriend;
        }

        public bool Delete(int id)
        {
            AppUser newFriend = this.TripickContext.Users.Where(x => x.Id == id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (newFriend == null)
                throw new NullReferenceException($"The user [Id={id}] does not exist");

            AppUser user = this.TripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            if (user.Friendships.Any(x => x.IdFriend == newFriend.Id))
                user.Friendships.RemoveAt(user.Friendships.IndexOf(user.Friendships.First(x => x.IdFriend == newFriend.Id)));

            if (newFriend.Friendships.Any(x => x.IdFriend == user.Id))
                newFriend.Friendships.RemoveAt(newFriend.Friendships.IndexOf(newFriend.Friendships.First(x => x.IdFriend == user.Id)));

            this.TripickContext.SaveChanges();
            return true;
        }

        public List<Friend> Search(string userName)
        {
            if (userName == null)
                return new List<Friend>();

            AppUser user = this.TripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            List<int> toExclude = (user.Friendships.Select(x => x.IdFriend).ToList()).Union(new List<int>() { user.Id }).ToList();
            List<AppUser> users = this.TripickContext.Users
                .Where(x => !toExclude.Contains(x.Id) && x.UserName.ToLower().StartsWith(userName.ToLower()))
                .Take(10)
                .ToList();
            return users.Select(u => new Friend(u)).ToList();
        }

        public List<Friend> Sync(List<Friend> friends)
        {
            AppUser user = this.TripickContext.Users.Where(x => x.Id == this.ConnectedUser().Id)
                .Include(t => t.Friendships)
                .SingleOrDefault();
            if (user == null)
                throw new NullReferenceException($"The user [Id={this.ConnectedUser().Id}] does not exist");

            List<int> ids = user.Friendships.Select(fs => fs.IdFriend).ToList();
            List<AppUser> friendsUsers = this.TripickContext.Users.Where(x => ids.Contains(x.Id)).Include(t => t.Photo).ToList();
            List<Friend> upToDateFriends = friendsUsers.Select(x => new Friend(x, user.Friendships.First(fs => fs.IdFriend == x.Id))).ToList();
            List<Friend> needSyncFriends = new List<Friend>();
            upToDateFriends.ForEach(nf =>
            {
                // Send all friends but don't send photos when there is no change (faster and lighter)
                Friend existingFriend = friends.FirstOrDefault(x => x.Id == nf.Id);
                if (nf.Equals(existingFriend))
                {
                    nf.Photo = null;
                    nf.SharedTrips = null;
                }
                needSyncFriends.Add(nf);
            });

            return needSyncFriends.OrderBy(f => f.UserName).ToList();
        }

        #endregion
    }
}
