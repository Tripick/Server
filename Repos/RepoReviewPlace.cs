using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoReviewPlace : RepoCRUD<ReviewPlace>
    {
        #region Constructor

        public RepoReviewPlace(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public ReviewPlace Get(int idPlace)
        {
            return this.TripickContext.ReviewPlace.Where(t => t.IdAuthor == this.ConnectedUser().Id && t.IdPlace == idPlace).FirstOrDefault();
        }
    }
}
