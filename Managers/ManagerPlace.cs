using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            places = places.Select(p => new Place() { Id=p.Id, Name=text.ToLower().StartsWith(p.Name.ToLower()) ? p.Name : p.NameTranslated }).ToList();
            return places;
        }

        public Place GetPlace(int id)
        {
            Place place = this.repoPlace.GetComplete(id);
            return place;
        }

        public List<ReviewPlace> Review(int idPlace, int rating, string message, List<ReviewFlag> flags, List<string> pictures)
        {
            // Get existing if any
            ReviewPlace review = this.repoReviewPlace.Get(idPlace);
            List<ConfigReviewFlag> configFlags = this.TripickContext.ConfigReviewFlags.ToList();
            flags = flags.Where(f => configFlags.Any(fConfig => fConfig.Id == f.IdReviewFlagConfig)).ToList();
            if (review != null)
            {
                // Clear previous images and flags
                this.TripickContext.ReviewFlags.RemoveRange(this.TripickContext.ReviewFlags.Where(f => f.IdReview == review.Id).ToList());
                this.TripickContext.ReviewImages.RemoveRange(this.TripickContext.ReviewImages.Where(i => i.IdReview == review.Id).ToList());
                this.TripickContext.SaveChanges();
                // Update the existing review
                review.Rating = Math.Max(0, Math.Min(5, rating));
                review.Message = message.ToCleanString();
                review.Flags = flags;
                review.Flags.ForEach(f => f.IdReview = review.Id);
                review.Pictures = pictures.Select(p => new ReviewImage() { IdReview = review.Id, Image = p }).ToList();
            }
            else
            {
                // Create a new review
                review = new ReviewPlace();
                review.CreationDate = DateTime.Now;
                review.IdPlace = idPlace;
                review.IdAuthor = this.ConnectedUser().Id;
                review.Rating = Math.Max(0, Math.Min(5, rating));
                review.Message = message.ToCleanString();
                review.Flags = flags;
                review.Pictures = pictures.Select(p => new ReviewImage() { Image = p }).ToList();
                this.repoReviewPlace.Add(review);
            }

            // Commit
            this.TripickContext.SaveChanges();
            return this.repoPlace.GetReviews(idPlace);
        }

        #endregion
    }
}
