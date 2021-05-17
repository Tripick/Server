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
            List<ConfigFlag> configFlags = this.TripickContext.ConfigReviewFlags.ToList();
            flags = flags.Where(f => configFlags.Any(fConfig => fConfig.Id == f.IdConfig)).ToList();
            // Update
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
            // Create
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

            this.updateRating(idPlace);
            this.updateFlags(idPlace);

            return noReturn ? null : this.repoPlace.GetReviews(idPlace);
        }

        #endregion

        #region Private

        private bool create(Place place)
        {
            Place newPlace = new Place()
            {
                Name = place.Name,
                NameTranslated = place.Name,
                CountryId = place.CountryId,
                Country = place.Country,
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
                existingPlace.CountryId = place.CountryId;
                existingPlace.Country = place.Country;
                existingPlace.Latitude = place.Latitude;
                existingPlace.Longitude = place.Longitude;
            }

            // Commit
            this.TripickContext.SaveChanges();

            ReviewPlace review = place.Reviews[0];
            Review(existingPlace.Id, review.Rating, review.Message, review.Flags, review.Pictures.Select(p => p.Image).ToList(), noReturn: true);
            return true;
        }

        private bool updateRating(int idPlace)
        {
            Place existingPlace = this.repoPlace.GetById(idPlace);
            if (existingPlace == null)
                throw new NullReferenceException("The place to update flags does not exist.");

            List<ReviewPlace> reviews = this.repoPlace.GetReviews(idPlace, withImages: false);
            existingPlace.Rating = reviews.Average(r => r.Rating);
            existingPlace.NbRating = reviews.Count;

            // Commit
            this.TripickContext.SaveChanges();
            return true;
        }

        private bool updateFlags(int idPlace)
        {
            Place existingPlace = this.repoPlace.GetById(idPlace);
            if (existingPlace == null)
                throw new NullReferenceException("The place to update flags does not exist.");

            List<ReviewPlace> reviews = this.repoPlace.GetReviews(idPlace, withImages:false);
            Dictionary<int, List<ReviewFlag>> reviewFlags = reviews.SelectMany(r => r.Flags).GroupBy(f => f.Config.Id).ToDictionary(x => x.Key, x => x.ToList());
            List<PlaceFlag> placeFlags = reviewFlags.Keys.Select(k =>
            {
                if(reviewFlags[k].Count > 0)
                {
                    double avg = reviewFlags[k].Average(f => double.Parse(f.Value));
                    string newFlagValue = avg.ToString();
                    if (reviewFlags[k][0].Config.ValType != "Double")
                    {
                        double diff = avg - Math.Truncate(avg);
                        newFlagValue = (Math.Truncate(avg) + (diff >= 0.5 ? 1 : 0)).ToString();
                    }
                    else
                    {
                        newFlagValue = (Math.Truncate(avg * 100) / 100).ToString();
                    }
                    return new PlaceFlag()
                    {
                        IdPlace = idPlace,
                        IdConfig = reviewFlags[k][0].IdConfig,
                        Value = newFlagValue,
                    };
                }
                return null;
            }).Where(x => x != null).ToList();

            existingPlace.Flags = placeFlags;

            // Commit
            this.TripickContext.SaveChanges();
            return true;
        }

        #endregion
    }
}
