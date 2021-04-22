using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoPlace : RepoCRUD<Place>
    {
        #region Constructor

        public RepoPlace(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<Place> GetAllInAreas(List<BoundingBox> areas)
        {
            var query = PredicateBuilder.New<Place>();
            foreach (BoundingBox area in areas)
            {
                query = query.Or(p =>
                    area.MinLat < p.Latitude &&
                    area.MaxLat > p.Latitude &&
                    area.MinLon < p.Longitude &&
                    area.MaxLon > p.Longitude);
            }
            List<Place> places = this.TripickContext.Places.AsExpandable().Where(query).Include(p => p.Images).ToList();
            return places;
        }

        public List<Place> GetNextsToPick(List<int> alreadyShown, List<BoundingBox> areas, List<Filter> filters, int quantity)
        {
            // Places in area of Trip
            var query = PredicateBuilder.New<Place>();
            foreach (BoundingBox area in areas)
            {
                query = query.Or(p =>
                    area.MinLat < p.Latitude &&
                    area.MaxLat > p.Latitude &&
                    area.MinLon < p.Longitude &&
                    area.MaxLon > p.Longitude);
            }

            // Include places who don't have filter information yet
            if(filters == null)
            {
                query = query.And(p => !p.Price.HasValue && !p.Difficulty.HasValue && !p.Length.HasValue && !p.Duration.HasValue && !p.Touristy.HasValue);
            }
            // Apply filters
            else if(filters.Any())
            {
                int? minPrice = filters.FirstOrDefault(f => f.Name == "Price")?.Min;
                int? maxPrice = filters.FirstOrDefault(f => f.Name == "Price")?.Max;
                if (minPrice.HasValue && maxPrice.HasValue)
                    query = query.And(p => p.Price >= minPrice.Value && p.Price <= maxPrice.Value);

                int? minDifficulty = filters.FirstOrDefault(f => f.Name == "Difficulty")?.Min;
                int? maxDifficulty = filters.FirstOrDefault(f => f.Name == "Difficulty")?.Max;
                if (minDifficulty.HasValue && maxDifficulty.HasValue)
                    query = query.And(p => p.Difficulty >= minDifficulty.Value && p.Difficulty <= maxDifficulty.Value);

                int? minLength = filters.FirstOrDefault(f => f.Name == "Length")?.Min;
                int? maxLength = filters.FirstOrDefault(f => f.Name == "Length")?.Max;
                if (minLength.HasValue && maxLength.HasValue)
                    query = query.And(p => p.Length >= minLength.Value && p.Length <= maxLength.Value);

                int? minDuration = filters.FirstOrDefault(f => f.Name == "Duration")?.Min;
                int? maxDuration = filters.FirstOrDefault(f => f.Name == "Duration")?.Max;
                if (minDuration.HasValue && maxDuration.HasValue)
                    query = query.And(p => p.Duration >= minDuration.Value && p.Duration <= maxDuration.Value);

                int? minTouristy = filters.FirstOrDefault(f => f.Name == "Touristy")?.Min;
                int? maxTouristy = filters.FirstOrDefault(f => f.Name == "Touristy")?.Max;
                if (minTouristy.HasValue && maxTouristy.HasValue)
                    query = query.And(p => p.Touristy >= minTouristy.Value && p.Touristy <= maxTouristy.Value);
            }

            // Skip those who were already picked before
            query = query.And(p => !alreadyShown.Contains(p.Id));

            // Get places according to query
            List<Place> places = this.TripickContext.Places.AsExpandable()
                .Where(query)
                .OrderByDescending(p => p.Rating)
                .Take(quantity)
                .ToList();

            // Include images and reviews
            foreach (Place place in places)
            {
                place.Images = this.TripickContext.ImagePlaces.Where(i => i.IdPlace == place.Id).ToList();
                place.Reviews = GetReviews(place.Id);
            }

            return places;
        }

        public int CountNextsToPick(List<int> alreadyShown, List<BoundingBox> areas, List<Filter> filters)
        {
            // Places in area of Trip
            var query = PredicateBuilder.New<Place>();
            foreach (BoundingBox area in areas)
            {
                query = query.Or(p =>
                    area.MinLat < p.Latitude &&
                    area.MaxLat > p.Latitude &&
                    area.MinLon < p.Longitude &&
                    area.MaxLon > p.Longitude);
            }

            // Include places who don't have filter information yet
            if (filters == null)
            {
                query = query.And(p => !p.Price.HasValue && !p.Difficulty.HasValue && !p.Length.HasValue && !p.Duration.HasValue && !p.Touristy.HasValue);
            }
            // Apply filters
            else if (filters.Any())
            {
                int? minPrice = filters.FirstOrDefault(f => f.Name == "Price")?.Min;
                int? maxPrice = filters.FirstOrDefault(f => f.Name == "Price")?.Max;
                if (minPrice.HasValue && maxPrice.HasValue)
                    query = query.And(p => p.Price >= minPrice.Value && p.Price <= maxPrice.Value);

                int? minDifficulty = filters.FirstOrDefault(f => f.Name == "Difficulty")?.Min;
                int? maxDifficulty = filters.FirstOrDefault(f => f.Name == "Difficulty")?.Max;
                if (minDifficulty.HasValue && maxDifficulty.HasValue)
                    query = query.And(p => p.Difficulty >= minDifficulty.Value && p.Difficulty <= maxDifficulty.Value);

                int? minLength = filters.FirstOrDefault(f => f.Name == "Length")?.Min;
                int? maxLength = filters.FirstOrDefault(f => f.Name == "Length")?.Max;
                if (minLength.HasValue && maxLength.HasValue)
                    query = query.And(p => p.Length >= minLength.Value && p.Length <= maxLength.Value);

                int? minDuration = filters.FirstOrDefault(f => f.Name == "Duration")?.Min;
                int? maxDuration = filters.FirstOrDefault(f => f.Name == "Duration")?.Max;
                if (minDuration.HasValue && maxDuration.HasValue)
                    query = query.And(p => p.Duration >= minDuration.Value && p.Duration <= maxDuration.Value);

                int? minTouristy = filters.FirstOrDefault(f => f.Name == "Touristy")?.Min;
                int? maxTouristy = filters.FirstOrDefault(f => f.Name == "Touristy")?.Max;
                if (minTouristy.HasValue && maxTouristy.HasValue)
                    query = query.And(p => p.Touristy >= minTouristy.Value && p.Touristy <= maxTouristy.Value);
            }

            // Skip those who were already picked before
            query = query.And(p => !alreadyShown.Contains(p.Id));

            // Get places according to query
            return this.TripickContext.Places.AsExpandable().Where(query).Count();
        }

        public List<Place> SearchAutocomplete(string text, int quantity)
        {
            List<Place> places = this.TripickContext.Places
                .Where(p => p.Name.StartsWith(text) || p.NameTranslated.StartsWith(text))
                .Take(quantity)
                .OrderByDescending(x => x.NbRating)
                .ToList();
            return places;
        }

        public List<ReviewPlace> GetReviews(int idPlace)
        {
            List<ReviewPlace> reviews = this.TripickContext.ReviewPlace.Where(r => r.IdPlace == idPlace).Include(r => r.Author).ThenInclude(a => a.Photo).ToList();
            reviews.ForEach(r =>
            {
                r.Place = null;
                r.Author = new AppUser()
                {
                    UserName = r.Author.UserName,
                    Photo = r.Author.Photo,
                };
            });
            return reviews;
        }
    }
}
