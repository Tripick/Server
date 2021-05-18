using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoReviewPlace : RepoCRUD<PlaceReview>
    {
        #region Constructor

        public RepoReviewPlace(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public PlaceReview Get(int idPlace)
        {
            return this.TripickContext.PlaceReviews.Where(t => t.IdAuthor == this.ConnectedUser().Id && t.IdPlace == idPlace).FirstOrDefault();
        }
    }
}
