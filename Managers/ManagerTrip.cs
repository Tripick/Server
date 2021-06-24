using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripickServer.Models;
using TripickServer.Utils;
using TripickServer.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using TripickServer.Models.Common;

namespace TripickServer.Managers
{
    public class ManagerTrip : ManagerBase
    {
        #region Properties

        private RepoConfiguration repoConfiguration;
        private RepoTrip repoTrip;
        private RepoPick repoPick;
        private RepoFilter repoFilter;

        #endregion

        #region Constructor

        public ManagerTrip(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoConfiguration = new RepoConfiguration(this.ConnectedUser, tripickContext);
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoPick = new RepoPick(this.ConnectedUser, tripickContext);
            this.repoFilter = new RepoFilter(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            List<Trip> trips = this.repoTrip.GetAll(pageIndex, pageSize).OrderByDescending(x => x.CreationDate).ToList();
            trips.ForEach(trip =>
            {
                trip.NbPicks = this.repoPick.CountNotZeroByTrip(trip.Id);
                trip.Filters = this.repoFilter.Get(f => f.IdTrip == trip.Id && f.IdUser == this.ConnectedUser().Id).ToList();
                if (!trip.Filters.Any())
                {
                    trip.Filters = new List<Filter>()
                    {
                        new Filter() { IdTrip = trip.Id, IdUser = this.ConnectedUser().Id, Name="Price", Min = 0, Max = 99 },
                        new Filter() { IdTrip = trip.Id, IdUser = this.ConnectedUser().Id, Name = "Length", Min = 0, Max = 300 },
                        new Filter() { IdTrip = trip.Id, IdUser = this.ConnectedUser().Id, Name = "Duration", Min = 0, Max = 999 },
                        new Filter() { IdTrip = trip.Id, IdUser = this.ConnectedUser().Id, Name = "Difficulty", Min = 0, Max = 5 },
                        new Filter() { IdTrip = trip.Id, IdUser = this.ConnectedUser().Id, Name = "Touristy", Min = 0, Max = 5 }
                    };
                }
            });
            return trips;
        }

        public Trip GetById(int id)
        {
            Trip trip = this.repoTrip.GetById(id);
            trip.NbPicks = this.repoPick.CountNotZeroByTrip(id);
            return trip;
        }

        public Trip Create()
        {
            int numberOfTrips = this.repoTrip.Count() % 5;
            Configuration configs = repoConfiguration.Get("TripCoverImage" + numberOfTrips);
            // Create
            Trip trip = new Trip()
            {
                IdOwner = this.ConnectedUser().Id,
                IsPublic = false,
                CoverImage = configs?.Value,
                Name = "My new trip",
                Description = string.Empty,
                Note = string.Empty
            };
            this.repoTrip.Add(trip);

            // Commit
            this.TripickContext.SaveChanges();
            return trip;
        }

        public Trip Update(Trip trip)
        {
            // Verify the entity to update is not null
            if (trip == null)
                throw new NullReferenceException("The trip to update cannot be null");

            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetByIdWithTiles(trip.Id);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={trip.Id}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            //CoverImage = existing.CoverImage;
            existing.IsPublic = trip.IsPublic;
            existing.Name = trip.Name;
            existing.Description = trip.Description;
            existing.Note = trip.Note;

            existing.StartDate = trip.StartDate;
            existing.StartLatitude = trip.StartLatitude;
            existing.StartLongitude = trip.StartLongitude;
            existing.StartLatitudeDelta = trip.StartLatitudeDelta;
            existing.StartLongitudeDelta = trip.StartLongitudeDelta;

            existing.EndDate = trip.EndDate;
            existing.EndLatitude = trip.EndLatitude;
            existing.EndLongitude = trip.EndLongitude;
            existing.EndLatitudeDelta = trip.EndLatitudeDelta;
            existing.EndLongitudeDelta = trip.EndLongitudeDelta;

            if (existing.Region != null)
            {
                existing.Region.Latitude = trip.Region.Latitude;
                existing.Region.Longitude = trip.Region.Longitude;
                existing.Region.LatitudeDelta = trip.Region.LatitudeDelta;
                existing.Region.LongitudeDelta = trip.Region.LongitudeDelta;
            }
            else
                existing.Region = trip.Region;
            if(trip.Polygon != null)
            {
                for (int i = 0; i < trip.Polygon.Count; i++)
                {
                    trip.Polygon[i].Index = i;
                }
            }
            existing.Polygon = trip.Polygon;
            existing.NbPicks = this.repoPick.CountNotZeroByTrip(trip.Id);

            if (trip.Tiles != null && trip.Tiles.Any())
                existing.Tiles = trip.Tiles;

            // Commit
            this.TripickContext.SaveChanges();
            return existing;
        }

        public Itinerary GetItinerary(int idTrip, bool regenerate)
        {
            // Get trip
            Trip trip = this.repoTrip.GetById(idTrip);
            int nbDays = Convert.ToInt32((trip.EndDate.Value - trip.StartDate.Value).TotalDays);

            // Get all picks that weren't rated 0/5
            List<Pick> picks = this.repoPick.GetPicked(idTrip).OrderByDescending(p => p.Rating).ToList();

            // PASSAGES : is list of all locations the user will go through => Start location + End location + Best picks
            List<Passage> passages = new List<Passage>();
            passages.Add(new Passage() { IsStart = true, Latitude = trip.StartLatitude.Value, Longitude = trip.StartLongitude.Value });
            passages.AddRange(picks.Take(nbDays).Select(p => new Passage() { Pick = p, Latitude = p.Place.Latitude, Longitude = p.Place.Longitude }).ToList());
            passages.Add(new Passage() { IsEnd = true, Latitude = trip.EndLatitude.Value, Longitude = trip.EndLongitude.Value });

            // Take all picks who are not passages, calculate minimal distance between a pick and its closest passage
            // Save them as "VISITS" in their closest passage
            List<ClosestPassage> closestPassages = new List<ClosestPassage>();
            List<int> passagesIds = passages.Where(p => !p.IsStart && !p.IsEnd && p.Pick != null).Select(p => p.Pick.Id).ToList();
            List<Pick> notPassages = picks.Where(p => !passagesIds.Contains(p.Id)).ToList();
            notPassages.ForEach(np =>
            {
                ClosestPassage closestPassage = new ClosestPassage();
                closestPassage.Visit = np;
                closestPassage.Distance = -1;
                foreach (Passage passage in passages)
                {
                    double distance = Functions.CoordinatesDistance(
                        startLat: passage.Latitude,
                        startLon: passage.Longitude,
                        endLat: np.Place.Latitude,
                        endLon: np.Place.Longitude);
                    if (closestPassage.Distance == -1 || distance < closestPassage.Distance)
                    {
                        closestPassage.Distance = distance;
                        closestPassage.Passage = passage;
                    }
                }
                closestPassages.Add(closestPassage);
            });
            Dictionary<Passage, List<ClosestPassage>> visits = closestPassages.GroupBy(x => x.Passage).ToDictionary(x => x.Key, x => x.OrderBy(c => c.Distance).ToList());
            List<Passage> passagesWithoutVisit = passages.Where(p => !visits.Keys.Contains(p)).ToList();
            passagesWithoutVisit.ForEach(pwv => visits.Add(pwv, new List<ClosestPassage>()));

            // Calculate the "VISIT_LIKELY" of each pick : it's the interest of a pick multiplied by the distance to get there
            // Must be between 0.5 and 1.5 (result must be between 0.1 and 4.9, not to be eliminated or better than a passage)
            // The lower the distance, the higher the ponderation
            // The ponderation can be between -2.5 and +2.5 (but the rating must be 0 < rating < 5)
            double avgDistance = closestPassages.Average(x => x.Distance);
            double ponderationFactor = 2.5;
            foreach (Passage passage in visits.Keys)
            {
                foreach (ClosestPassage cp in visits[passage])
                {
                    double ratingModfier = Math.Max(-ponderationFactor, Math.Min(ponderationFactor, ponderationFactor * (cp.Distance / avgDistance) - ponderationFactor));
                    cp.VisitLikely = Math.Max(0.1, Math.Min(4.9, cp.Visit.Rating + ratingModfier));
                }
            }

            // Now that each day has at least one activity, we can add 2 additional visits to do each day
            // Finally, once each day have 1 to 3 visits :
            // Add all other visits of the passage as "suggestions" in the suggestion list of each day that is or follows a passage, ordered by ponderation
            List<ItineraryDay> days = new List<ItineraryDay>();
            int index = 0;
            foreach (Passage passage in visits.Keys)
            {
                List<ItineraryDayStep> steps = new List<ItineraryDayStep>();
                // Add passage
                steps.Add(new ItineraryDayStep()
                {
                    Index = 0,
                    IsPassage = true,
                    IsStart = passage.IsStart,
                    IsEnd = passage.IsEnd,
                    Latitude = passage.IsStart ? trip.StartLatitude.Value : (passage.IsEnd ? trip.EndLatitude.Value : 0),
                    Longitude = passage.IsStart ? trip.StartLongitude.Value : (passage.IsEnd ? trip.EndLongitude.Value : 0),
                    IsVisit = true,
                    Visit = passage.Pick,
                    VisitLikely = 5
                });
                // Add other picks
                int indexStep = 0;
                foreach (ClosestPassage cp in visits[passage])
                {
                    indexStep++;
                    steps.Add(new ItineraryDayStep()
                    {
                        Index = indexStep,
                        IsPassage = false,
                        IsVisit = indexStep < 3,
                        IsSuggestion = indexStep >= 3,
                        Visit = cp.Visit,
                        VisitLikely = cp.VisitLikely,
                        DistanceToPassage = cp.Distance
                    });
                }
                days.Add(new ItineraryDay() { Index = index, Steps = steps });
                index++;
            }

            // Pour l'algo d'ordre des jours :
            // Calculer la distance de chaque point vers chaque point
            // Sommer la distance totale parcouru pour chaque chemin possible
            // Changer de chemin lorsque la pondération des chemins est plus faible que le chemin courant
            // NOTE : if there is more golden than the number of days, some golden must be grouped (maybe the closest ones can be done at once)
            // Or remove those who are too far away and add them as visit suggestions
            List<ItineraryDay> orderedDays = new List<ItineraryDay>();
            orderedDays.Add(days.Where(d => d.Steps[0].IsStart).First());
            orderedDays.AddRange(days.Where(d => !d.Steps[0].IsStart && !d.Steps[0].IsEnd).ToList());
            orderedDays.Add(days.Where(d => d.Steps[0].IsEnd).First());
            days = orderedDays;

            List<ItineraryDay> bestOrderedDays = findBestOrder(days);

            Itinerary iti = new Itinerary() { };
            //iti.Days = bestOrderedDays;
            return iti;
        }

        private List<ItineraryDay> findBestOrder(List<ItineraryDay> days)
        {
            ItineraryDay start = days.Where(d => d.Steps[0].IsStart).FirstOrDefault();
            ItineraryDay end = days.Where(d => d.Steps[0].IsEnd).FirstOrDefault();
            days = days.Where(d => !d.Steps[0].IsStart && !d.Steps[0].IsEnd).ToList();
            foreach (ItineraryDay day in days)
            {
                day.DistanceToStart = Functions.CoordinatesDistance(start.Steps[0].Latitude, start.Steps[0].Longitude, day.Steps[0].Visit.Place.Latitude, day.Steps[0].Visit.Place.Longitude);
                day.DistanceToEnd = Functions.CoordinatesDistance(end.Steps[0].Latitude, end.Steps[0].Longitude, day.Steps[0].Visit.Place.Latitude, day.Steps[0].Visit.Place.Longitude);
            }

            days = days.OrderBy(d => d.DistanceToStart - d.DistanceToEnd).ToList();
            days.Add(end);
            days.Insert(0, start);
            return days;
        }

        private ItineraryDaysOrder findBestOrder2(List<ItineraryDay> days, List<ItineraryDaysOrder> orders)
        {
            // The current order is the one with the smallest distance
            ItineraryDaysOrder order = orders.OrderBy(o => o.TotalDistance).First();
            // The current day is the last day added in the order
            ItineraryDay day = order.Days.Last();
            // Take all days who are not already in the order
            List<ItineraryDay> daysLeft = days.Where(d => !order.Days.Any(od => od.Index == d.Index)).ToList();
            // When there's is no days left to add -> return the best order
            if (daysLeft.Count == 0)
                return order;
            else
                orders.Remove(order); // The current order must be eliminated to avoid doing the same step again

            // Calculate distance to all days that are not already in our "shorter" order
            double lat = day.Steps[0].IsStart || day.Steps[0].IsEnd ? day.Steps[0].Latitude : day.Steps[0].Visit.Place.Latitude;
            double lon = day.Steps[0].IsStart || day.Steps[0].IsEnd ? day.Steps[0].Longitude : day.Steps[0].Visit.Place.Longitude;
            List<Tuple<double, ItineraryDay>> distanceToDays = calculateDistanceToDays(lat, lon, daysLeft).ToList();
            distanceToDays.ForEach(dtd => orders.Add(new ItineraryDaysOrder()
            {
                TotalDistance = order.TotalDistance + dtd.Item1,
                Days = order.Days.Append(dtd.Item2).ToList()
            }));

            // Repeat the process with the new orders
            return findBestOrder2(days, orders);
        }

        private List<Tuple<double, ItineraryDay>> calculateDistanceToDays(double lat, double lon, List<ItineraryDay> days)
        {
            // If day.IsEnd == true -> return infinity distance EXCEPT if days.Count == 1
            // This way we are sure to finish by the last passage on the last day (and the algorithm continues if there is another path with smaller distance)
            if (days.Count == 1 && days.First().Steps[0].IsEnd)
            {
                double distance = Functions.CoordinatesDistance(lat, lon, days.First().Steps[0].Latitude, days.First().Steps[0].Longitude);
                return new List<Tuple<double, ItineraryDay>>() { new Tuple<double, ItineraryDay>(distance, days.First()) };
            }

            // Do not calculate the distance with the end, instead, simulate that the end is unreachable until the last node
            ItineraryDay end = days.Where(d => d.Steps[0].IsEnd).FirstOrDefault();
            List<Tuple<double, ItineraryDay>> distances = new List<Tuple<double, ItineraryDay>>() { new Tuple<double, ItineraryDay>(double.PositiveInfinity, end) };

            // Then calculate the distance with the rest of the days
            days = days.Where(d => !d.Steps[0].IsEnd).ToList();
            foreach (ItineraryDay day in days)
            {
                double distance = Functions.CoordinatesDistance(lat, lon, day.Steps[0].Visit.Place.Latitude, day.Steps[0].Visit.Place.Longitude);
                distances.Add(new Tuple<double, ItineraryDay>(distance, day));
            }
            return distances;
        }

        public bool Delete(int id)
        {
            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetById(id);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={id}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            // Delete
            this.repoTrip.Delete(existing);

            // Commit
            this.TripickContext.SaveChanges();
            return true;
        }

        public string SaveCover(int idTrip, string cover)
        {
            // Verify the entity to update is not null
            if (cover == null)
                throw new NullReferenceException("Cover photo required");

            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetById(idTrip);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={idTrip}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            existing.CoverImage = cover;

            // Commit
            this.TripickContext.SaveChanges();
            return existing.CoverImage;
        }

        #endregion
    }
}
