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
        private RepoCountry repoCountry;

        #endregion

        #region Constructor

        public ManagerPlace(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoPlace = new RepoPlace(this.ConnectedUser, tripickContext);
            this.repoReviewPlace = new RepoReviewPlace(this.ConnectedUser, tripickContext);
            this.repoCountry = new RepoCountry(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public List<Place> SearchAutocomplete(string text, int quantity)
        {
            List<Place> places = this.repoPlace.SearchAutocomplete(text, Math.Min(10, quantity));
            places = places.Select(p => new Place()
            {
                Id = p.Id,
                Country = p.Country,
                Name = text.ToLower().StartsWith(p.Name.ToLower()) ? p.Name : p.NameTranslated
            }).ToList();
            return places;
        }

        public bool Save(Place place)
        {
            // Verify the entity to update is not null
            if (place == null)
                throw new NullReferenceException("The place to save cannot be null");

            // Check if the entity to update exists or not
            if (place.Id == -1)
                return create(place);
            else
                return update(place);
        }

        public Place GetPlace(int id)
        {
            Place place = this.repoPlace.GetComplete(id);
            return place;
        }

        public List<ReviewPlace> Review(int idPlace, int rating, string message, List<ReviewFlag> flags, List<string> pictures, bool noReturn = false)
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
            return noReturn ? null : this.repoPlace.GetReviews(idPlace);
        }

        public List<Country> GetCountries(int quantity)
        {
            return this.repoCountry.GetAll(quantity);
        }

        #endregion

        #region Private

        private bool create(Place place)
        {
            Country country = this.repoCountry.GetCountryOfPosition(place.Latitude, place.Longitude);
            Place newPlace = new Place()
            {
                Name = place.Name,
                NameTranslated = place.Name,
                Country = country.Name,
                CreationDate = DateTime.Now,
                Description = place.Description,
                Latitude = place.Latitude,
                Longitude = place.Longitude
            };

            this.repoPlace.Add(newPlace);

            // Commit
            this.TripickContext.SaveChanges();

            ReviewPlace review = place.Reviews[0];
            Review(newPlace.Id, review.Rating, review.Message, review.Flags, review.Pictures.Select(p => p.Image).ToList(), noReturn: true);
            return true;
        }

        private bool update(Place place)
        {
            Place existingPlace = this.repoPlace.GetById(place.Id);
            if(existingPlace == null)
                throw new NullReferenceException("The place to update cannot be null");

            existingPlace.Name = place.Name;
            existingPlace.NameTranslated = place.Name;
            existingPlace.Description = place.Description;
            if(place.Latitude != existingPlace.Latitude || place.Longitude != existingPlace.Longitude)
            {
                existingPlace.Country = this.repoCountry.GetCountryOfPosition(place.Latitude, place.Longitude).Name;
                existingPlace.Latitude = place.Latitude;
                existingPlace.Longitude = place.Longitude;
            }

            // Commit
            this.TripickContext.SaveChanges();

            ReviewPlace review = place.Reviews[0];
            Review(existingPlace.Id, review.Rating, review.Message, review.Flags, review.Pictures.Select(p => p.Image).ToList(), noReturn: true);
            return true;
        }

        #endregion
    }
}
