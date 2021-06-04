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
            place.Images.ForEach(i => { i.Place = null; i.Uploader = null; });
            place.Flags.ForEach(r => { r.Place = null; });
            place.Reviews.ForEach(r =>
            {
                r.Place = null;
                r.Author = new AppUser() { UserName = r.Author.UserName, Photo = r.Author.Photo };
                r.Flags.ForEach(f => f.Review = null);
                r.Pictures.ForEach(p => p.Review = null);
            });
            return place;
        }

        public List<PlaceReview> Review(int idPlace, double rating, string message, List<ReviewFlag> flags, List<string> pictures, bool noReturn = false)
        {
            // Add a "system review" if there is no review on that place yet (in order to keep the current rating of a place)
            List<PlaceReview> allExistingReviews = this.repoPlace.GetReviews(idPlace, withImages:false);
            if(!allExistingReviews.Any())
            {
                Place place = this.repoPlace.GetById(idPlace);
                PlaceReview systemReview = new PlaceReview();
                systemReview.CreationDate = DateTime.Now;
                systemReview.IdPlace = idPlace;
                systemReview.IdAuthor = 2;
                systemReview.Rating = Math.Max(0, Math.Min(5, place.Rating));
                systemReview.Message = " ";
                systemReview.Flags = new List<ReviewFlag>();
                systemReview.Pictures = new List<ReviewImage>();
                this.repoReviewPlace.Add(systemReview);
                this.TripickContext.SaveChanges();
            }

            // Get existing if any
            PlaceReview review = allExistingReviews.FirstOrDefault(r => r.IdAuthor == this.ConnectedUser().Id);
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
                review = new PlaceReview();
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
            this.updateRatingAndFlags(idPlace);

            if (noReturn)
               return null;

            List<PlaceReview> reviews = this.repoPlace.GetReviews(idPlace);
            reviews.ForEach(r =>
            {
                r.Place = null;
                r.Author = new AppUser() { UserName = r.Author.UserName, Photo = r.Author.Photo };
                r.Flags.ForEach(f => f.Review = null);
                r.Pictures.ForEach(p => p.Review = null);
            });
            return reviews;
        }

        #endregion

        #region Generation

        public Place GenerationSaveimages(int id, List<string> urls)
        {
            if(id == -1)
            {
                return generationGetNextPlace();
            }
            else
            {
                generationSaveImages(id, urls);
                return generationGetNextPlace();
            }
        }

        private void generationSaveImages(int id, List<string> urls)
        {
            List<ImagePlace> images = urls.Select(u => new ImagePlace()
            {
                IdPlace = id,
                IdUploader = 2,
                IsCover = true,
                IsVerified = true,
                Url = u,
                CreationDate = DateTime.Now,
            }).ToList();
            this.TripickContext.ImagePlaces.AddRange(images);
            this.TripickContext.SaveChanges();
        }

        private Place generationGetNextPlace()
        {
            Place place = this.TripickContext.Places.Where(p => !string.IsNullOrWhiteSpace(p.PlaceId) && !p.Images.Any()).FirstOrDefault();
            if (place == null)
                return new Place() { Id = -1 };
            return new Place() { Id = place.Id, PlaceId = place.PlaceId };
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

            PlaceReview review = place.Reviews[0];
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

            PlaceReview review = place.Reviews[0];
            Review(existingPlace.Id, review.Rating, review.Message, review.Flags, review.Pictures.Select(p => p.Image).ToList(), noReturn: true);
            return true;
        }

        private bool updateRatingAndFlags(int idPlace)
        {
            Place existingPlace = this.repoPlace.GetComplete(idPlace);
            if (existingPlace == null)
                throw new NullReferenceException("The place to update flags does not exist.");

            List<PlaceReview> reviews = this.repoPlace.GetReviews(idPlace, withImages: false);
            existingPlace.Rating = reviews.Average(r => r.Rating);
            existingPlace.NbRating = reviews.Count;

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
