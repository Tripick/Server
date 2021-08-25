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
        private RepoPlaceFlag repoFilter;
        private RepoItinerary repoItinerary;
        private RepoItineraryDay repoItineraryDay;

        #endregion

        #region Constructor

        public ManagerTrip(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoConfiguration = new RepoConfiguration(this.ConnectedUser, tripickContext);
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoPick = new RepoPick(this.ConnectedUser, tripickContext);
            this.repoFilter = new RepoPlaceFlag(this.ConnectedUser, tripickContext);
            this.repoItinerary = new RepoItinerary(this.ConnectedUser, tripickContext);
            this.repoItineraryDay = new RepoItineraryDay(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Private

        public List<Trip> GetAll(int pageIndex = 0, int pageSize = 10)
        {
            List<Trip> trips = this.repoTrip.GetAll(pageIndex, pageSize).OrderByDescending(x => x.CreationDate).ToList();
            trips.ForEach(trip =>
            {
                trip.NbPicks = this.repoPick.CountNotZeroByTrip(trip.Id);
                trip.Filters = this.repoFilter.GetFilters(trip.Id);
            });
            return trips;
        }

        public Trip GetById(int id)
        {
            Trip trip = this.repoTrip.GetById(id);
            trip.NbPicks = this.repoPick.CountNotZeroByTrip(id);
            return trip;
        }

        public Trip Create(string name, string photo)
        {
            //int numberOfTrips = this.repoTrip.Count() % 5;
            //Configuration configs = repoConfiguration.Get("TripCoverImage" + numberOfTrips);
            // Create
            Trip trip = new Trip()
            {
                IdOwner = this.ConnectedUser().Id,
                IsPublic = false,
                CoverImage = photo,
                Name = name,
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

            existing.NbPicks = this.repoPick.CountNotZeroByTrip(trip.Id);

            if(trip.Region != null)
            {
                existing.Region = new Location()
                {
                    IdTrip = existing.Id,
                    Latitude = trip.Region.Latitude,
                    Longitude = trip.Region.Longitude,
                    LatitudeDelta = trip.Region.LatitudeDelta,
                    LongitudeDelta = trip.Region.LongitudeDelta
                };
            }

            if(trip.Polygon != null)
            {
                for (int i = 0; i < trip.Polygon.Count; i++)
                {
                    trip.Polygon[i] = new MapPoint()
                    {
                        Index = i,
                        IdTrip = existing.Id,
                        Latitude = trip.Polygon[i].Latitude,
                        Longitude = trip.Polygon[i].Longitude
                    };
                }
            }
            existing.Polygon = trip.Polygon;

            if (trip.Tiles != null)
            {
                for (int i = 0; i < trip.Tiles.Count; i++)
                {
                    trip.Tiles[i] = new MapTile()
                    {
                        IdTrip = existing.Id,
                        Latitude = trip.Tiles[i].Latitude,
                        Longitude = trip.Tiles[i].Longitude,
                        Height = trip.Tiles[i].Height,
                        Width = trip.Tiles[i].Width
                    };
                }
            }
            existing.Tiles = trip.Tiles;

            // Commit
            this.TripickContext.SaveChanges();
            return existing;
        }

        #region Itinerary

        public Itinerary GetItinerary(int idTrip, bool regenerate)
        {
            Itinerary iti = this.TripickContext.Itineraries.FirstOrDefault(x => x.IdTrip == idTrip);
            if (iti != null)
                return this.repoItinerary.GetByIdTrip(idTrip);

            // Get trip
            Trip trip = this.repoTrip.GetById(idTrip);
            int nbDays = Convert.ToInt32((trip.EndDate.Value - trip.StartDate.Value).TotalDays);

            // Get all picks that weren't rated 0/5
            List<Pick> picks = this.repoPick.GetPicked(idTrip).OrderByDescending(p => p.Rating).ToList();

            // PASSAGES : is the list of all locations the user will absolutely go to => Start location + End location + Best picks
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
            double avgDistance = closestPassages.Any() ? closestPassages.Average(x => x.Distance) : 0;
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
                    Time = null,
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
                        Time = null,
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
            int i = 1;
            days.AddRange(Enumerable.Range(0, nbDays - days.Count).Select(n => new ItineraryDay() { Steps = new List<ItineraryDayStep>() }));
            foreach (ItineraryDay d in days)
            {
                d.Index = i;
                i++;
            }
            days = findBestOrder(days);
            // Remove start and end points from activities
            days.ForEach(d =>
            {
                d.Steps = d.Steps.Where(s => !s.IsStart && !s.IsEnd).OrderByDescending(s => s.Visit.Rating).ToList();
                if(d.Steps.Any()) d.Steps.First().IsPassage = true;
            });

            i = 0;
            foreach (ItineraryDay d in days)
            {
                d.Date = trip.StartDate.Value.Date.AddDays(i);
                int j = 0;
                foreach (ItineraryDayStep s in d.Steps.Where(s => s.IsPassage))
                {
                    s.Index = j;
                    s.Time = d.Date;
                    if (s.Visit != null)
                    {
                        s.IdVisit = s.Visit.Id;
                        s.Visit = null;
                    }
                    j++;
                }
                foreach (ItineraryDayStep s in d.Steps.Where(s => !s.IsPassage && s.IsVisit))
                {
                   s.Index = j;
                   s.Time = d.Date;
                   if (s.Visit != null)
                   {
                       s.IdVisit = s.Visit.Id;
                       s.Visit = null;
                    }
                   j++;
                }
                foreach (ItineraryDayStep s in d.Steps.Where(s => !s.IsPassage && !s.IsVisit && s.IsSuggestion))
                {
                    s.Index = j;
                    s.Time = d.Date;
                    if (s.Visit != null)
                    {
                        s.IdVisit = s.Visit.Id;
                        s.Visit = null;
                    }
                    j++;
                }
                i++;
            }

            //if (iti != null)
            //    this.TripickContext.ItineraryDays.RemoveRange(this.TripickContext.ItineraryDays.Where(x => x.IdItinerary == iti.Id).ToArray());
            //else
            iti = new Itinerary();
            iti.IdTrip = idTrip;
            iti.CreationDate = DateTime.Now;
            iti.Days = days;
            iti.IsActive = false;

            this.TripickContext.Itineraries.Add(iti);
            trip.IsItineraryGenerated = true;
            this.TripickContext.SaveChanges();

            return iti;
        }

        private List<ItineraryDay> findBestOrder(List<ItineraryDay> days)
        {
            ItineraryDay start = days.Where(d => d.Steps.Any() && d.Steps[0].IsStart).FirstOrDefault();
            start.Index = 0;
            ItineraryDay end = days.Where(d => d.Steps.Any() && d.Steps[0].IsEnd).FirstOrDefault();
            days = days.Where(d => !d.Steps.Any() || (!d.Steps[0].IsStart && !d.Steps[0].IsEnd)).ToList();
            List<ItineraryDay> order = new List<ItineraryDay>() { start };
            List<ItineraryDay> uncharteds = days.Where(d => d.Steps.Any()).ToList();
            List<ItineraryDay> emptyDays = days.Where(d => !d.Steps.Any()).ToList();
            ItineraryDay currentDay = start;
            int i = 1;
            while (order.Count < days.Count || uncharteds.Count <= 0)
            {
                int closestId = -1;
                double closestDistance = double.PositiveInfinity;
                if(currentDay.Steps.Any() && uncharteds.Any())
                {
                    foreach (ItineraryDay uncharted in uncharteds)
                    {
                        double distance = Functions.CoordinatesDistance(
                            currentDay.Steps[0].IsStart || currentDay.Steps[0].IsEnd ? currentDay.Steps[0].Latitude : currentDay.Steps[0].Visit.Place.Latitude,
                            currentDay.Steps[0].IsStart || currentDay.Steps[0].IsEnd ? currentDay.Steps[0].Longitude : currentDay.Steps[0].Visit.Place.Longitude,
                            uncharted.Steps[0].Visit.Place.Latitude,
                            uncharted.Steps[0].Visit.Place.Longitude);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestId = uncharted.Index;
                        }
                    }
                    ItineraryDay nextDay = uncharteds.FirstOrDefault(d => d.Index == closestId);
                    uncharteds.Remove(nextDay);
                    nextDay.Index = i;
                    order.Add(nextDay);
                    currentDay = nextDay;
                    i++;
                }
                else
                {
                    // Add the days that don't have any step at the end of the list
                    order.AddRange(emptyDays);
                    for (; i < order.Count; i++) { order[i].Index = i; }
                    break;
                }
            }

            end.Index = order.Count;
            order.Add(end);
            return order.OrderBy(d => d.Index).ToList();
        }

        private List<ItineraryDay> findBestOrderStartEnd(List<ItineraryDay> days)
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
            for (int i = 0; i < days.Count; i++)
            {
                days[i].Index = i;
            }
            days = days.OrderBy(d => d.Index).ToList();
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

        public bool SaveDays(int idTrip, List<ItineraryDay> days)
        {
            Trip trip = this.TripickContext.Trips.FirstOrDefault(x => x.Id == idTrip && x.IdOwner == this.ConnectedUser().Id);
            if (trip == null)
                throw new NullReferenceException("The trip to update is not yours.");

            Itinerary existingItinerary = this.repoItinerary.GetByIdTrip(idTrip);
            if (existingItinerary == null)
                throw new NullReferenceException("The itinerary to update does not exist.");

            // Delete those who are not in the new list of days
            ItineraryDay[] daysToDelete = existingItinerary.Days.Where(d => !days.Any(x => x.Id == d.Id)).ToArray();
            days = days.Where(d => !daysToDelete.Any((x => x.Id == d.Id))).OrderBy(d => d.Index).ToList();
            ItineraryDay startDay = days.First(d => d.Index == days.Min(dd => dd.Index));
            days.ForEach(d => d.Date = startDay.Date.AddDays(d.Index));
            for (int i = 0; i < days.Count; i++) { days[i].Index = i; } // Refresh days indexes
            if (daysToDelete.Any())
            {
                this.TripickContext.ItineraryDays.RemoveRange(daysToDelete);
                this.TripickContext.SaveChanges();
            }

            // Add those who are new in the list
            List<ItineraryDay> daysToAdd = days.Where(d => !existingItinerary.Days.Any(x => x.Id == d.Id)).Select(d => new ItineraryDay()
            {
                //DistanceToStart = 0,
                //DistanceToEnd = 0,
                Date = d.Date,
                IdItinerary = existingItinerary.Id,
                Index = d.Index,
                Name = d.Name,
                Steps = d.Steps == null ? new List<ItineraryDayStep>() : d.Steps.Select(s => new ItineraryDayStep()
                {
                    //IdDay = d.Id,
                    //DistanceToPassage = 0,
                    //Latitude = 0,
                    //Longitude = 0,
                    //VisitLikely = 0,
                    //IsEnd = false,
                    //IsStart = false,
                    //IsPassage = false,
                    //IsSuggestion = false,
                    IdVisit = s.IdVisit, // IdVisit always arrives as the Id of the place selected (need to find/create the pick related to this place)
                    Index = s.Index,
                    IsVisit = true,
                    Time = s.Time,
                }).ToList()
            }).ToList();

            // Find/Create pick related to each IdVisit of each step of each day
            daysToAdd.ForEach(dta =>
            {
                dta.Steps.ForEach(s =>
                {
                    int idPlace = s.IdVisit.Value; // Only start and end don't have an IdVisit (since there's no visit to do on start and end)
                    // Find the (potentially) existing pick for the place
                    Pick existingPick = this.repoPick.GetByIdPlace(idTrip, idPlace);
                    if (existingPick != null)
                        s.IdVisit = existingPick.Id;
                    else
                    {
                        Pick pickToCreate = new Pick()
                        {
                            // Index = 0, This is only used to order the picks to be displayed to the user on the pick page, no need for index here
                            IdTrip = idTrip,
                            IdPlace = idPlace,
                            IdUser = this.ConnectedUser().Id,
                            Rating = 5 // The place has been selected especially by the user, we assume that he really wants to go there so gets the maximum rate
                        };
                        this.TripickContext.Picks.Add(pickToCreate);
                        this.TripickContext.SaveChanges();
                        s.IdVisit = pickToCreate.Id;
                    }
                });
            });

            // Apply changes
            this.TripickContext.ItineraryDays.AddRange(daysToAdd.ToArray());
            existingItinerary.Days
                .Where(d => days.Any(dd => dd.Id == d.Id))
                .ToList()
                .ForEach(d => { d.Index = days.First(x => x.Id == d.Id).Index; d.Date = days.First(x => x.Id == d.Id).Date; });
            this.TripickContext.SaveChanges();
            return true;
        }

        public bool SaveDay(int idTrip, ItineraryDay day)
        {
            Trip trip = this.TripickContext.Trips.FirstOrDefault(x => x.Id == idTrip && x.IdOwner == this.ConnectedUser().Id);
            if (trip == null)
                throw new NullReferenceException("The trip to update is not yours.");

            ItineraryDay existingDay = this.repoItineraryDay.GetById(day.Id);
            if (existingDay == null)
                throw new NullReferenceException("The day to update does not exist.");

            existingDay.Name = day.Name;

            this.TripickContext.SaveChanges();
            return true;
        }

        public ItineraryDay SaveSteps(int idTrip, int idDay, List<ItineraryDayStep> steps)
        {
            Trip trip = this.TripickContext.Trips.FirstOrDefault(x => x.Id == idTrip && x.IdOwner == this.ConnectedUser().Id);
            if (trip == null)
                throw new NullReferenceException("The trip to update is not yours.");
            bool doesDayExist = this.repoItineraryDay.GetById(idDay) != null;
            if (!doesDayExist)
                throw new NullReferenceException("The day to update does not exist.");
            this.TripickContext.ItineraryDaySteps.RemoveRange(this.TripickContext.ItineraryDaySteps.Where(x => x.IdDay == idDay).ToArray());
            this.TripickContext.SaveChanges();

            ItineraryDay day = this.repoItineraryDay.GetWithSteps(idDay);
            day.Steps = steps.Select(s => new ItineraryDayStep()
            {
                DistanceToPassage = s.DistanceToPassage,
                IdDay = day.Id,
                IdVisit = s.IdVisit,
                Index = s.Index,
                IsEnd = s.IsEnd,
                IsStart = s.IsStart,
                IsPassage = s.IsPassage,
                IsSuggestion = s.IsSuggestion,
                IsVisit = s.IsVisit,
                Latitude = s.Latitude,
                Longitude = s.Longitude,
                Time = s.Time,
                VisitLikely = s.VisitLikely
            }).ToList();
            this.TripickContext.SaveChanges();
            return day;
        }

        public bool MoveStep(int idTrip, int idOldDay, int idNewDay, int idStep)
        {
            Trip trip = this.TripickContext.Trips.FirstOrDefault(x => x.Id == idTrip && x.IdOwner == this.ConnectedUser().Id);
            if (trip == null)
                throw new NullReferenceException("The trip to update is not yours.");
            ItineraryDay oldDay = this.repoItineraryDay.GetById(idOldDay);
            if (oldDay == null)
                throw new NullReferenceException("The old day to update does not exist.");
            ItineraryDay newDay = this.repoItineraryDay.GetById(idNewDay);
            if (newDay == null)
                throw new NullReferenceException("The new day to update does not exist.");
            ItineraryDayStep step = this.TripickContext.ItineraryDaySteps.Where(x => x.Id == idStep).FirstOrDefault();
            step.IdDay = newDay.Id;
            this.TripickContext.SaveChanges();
            return true;
        }

        #endregion

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

        public bool SaveFilters(int idTrip, List<PlaceFlag> filters)
        {
            // Verify the entity to update exists
            Trip existing = this.repoTrip.GetById(idTrip);
            if (existing == null)
                throw new NullReferenceException($"The trip [Id={idTrip}] does not exist");
            // Verify the entity is mine
            if (existing.IdOwner != this.ConnectedUser().Id)
                throw new NullReferenceException("The trip does not belong to you");

            this.repoFilter.Save(idTrip, filters);

            return true;
        }

        #endregion
    }
}
