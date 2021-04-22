using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TripickServer.Models;
using TripickServer.Repos;
using TripickServer.Utils;

namespace TripickServer.Managers
{
    public class ManagerPlace : ManagerBase
    {
        #region Properties

        private RepoPlace repoPlace;
        private RepoReviewPlace repoReviewPlace;

        #endregion

        #region Constructor

        public ManagerPlace(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoPlace = new RepoPlace(this.ConnectedUser, tripickContext);
            this.repoReviewPlace = new RepoReviewPlace(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public List<Place> SearchAutocomplete(string text, int quantity)
        {
            List<Place> places = this.repoPlace.SearchAutocomplete(text, Math.Min(10, quantity));
            return places;
        }

        public List<ReviewPlace> Review(int idPlace, int rating, string message, string title)
        {
            // Get existing if any
            ReviewPlace review = this.repoReviewPlace.Get(idPlace);
            if(review != null)
            {
                // Update the existing review
                review.Rating = Math.Max(0, Math.Min(5, rating));
                review.Message = message.ToCleanString();
                review.Title = title.ToCleanString();
            }
            else
            {
                // Create a new review
                review = new ReviewPlace();
                review.IdPlace = idPlace;
                review.IdAuthor = this.ConnectedUser().Id;
                review.Rating = Math.Max(0, Math.Min(5, rating));
                review.Message = message.ToCleanString();
                review.Title = title?.ToCleanString();
                this.repoReviewPlace.Add(review);
            }

            // Commit
            this.TripickContext.SaveChanges();
            List<ReviewPlace> reviews = this.repoPlace.GetById(idPlace).Reviews;
            reviews.ForEach(r => { r.Author = null; r.Place = null; });
            return reviews;
        }

        #endregion
    }
}
