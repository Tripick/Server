using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoPlaceFlag : RepoCRUD<PlaceFlag>
    {
        #region Constructor

        public RepoPlaceFlag(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<PlaceFlag> GetFilters(int idTrip)
        {
            List<PlaceFlag> filters = this.TripickContext.PlaceFlags
                .Where(p => p.IdTrip == idTrip)
                .Include(p => p.Config)
                .ToList();
            return filters;
        }

        public void Save(int idTrip, List<PlaceFlag> filters)
        {
            List<PlaceFlag> existingPlaceFlags = this.TripickContext.PlaceFlags
                .Where(p => p.IdTrip == idTrip)
                .Include(p => p.Config)
                .ToList();

            // Remove all filters who are not in the new list
            List<PlaceFlag> toRemove = existingPlaceFlags.Where(pf => !filters.Any(x => x.Config.Id == pf.Config.Id)).ToList();
            this.TripickContext.PlaceFlags.RemoveRange(toRemove);

            // Modify values of all those who still exist
            List<PlaceFlag> toModify = existingPlaceFlags.Where(pf => filters.Any(x => x.Config.Id == pf.Config.Id)).ToList();
            toModify.ForEach(epf =>
            {
                PlaceFlag newFilter = filters.FirstOrDefault(x => x.Config.Id == epf.Config.Id);
                if (newFilter != null)
                {
                    epf.Value = newFilter.Value;
                    epf.MaxValue = newFilter.MaxValue;
                }
            });

            // Add the new filters
            filters.Where(x => !existingPlaceFlags.Any(epf => epf.Config.Id == x.Config.Id)).ToList().ForEach(f =>
            {
                this.TripickContext.PlaceFlags.Add(new PlaceFlag()
                {
                    IdTrip = idTrip,
                    IdConfig = f.Config.Id,
                    Value = f.Value,
                    MaxValue = f.MaxValue
                });
            });

            // Commit
            this.TripickContext.SaveChanges();
        }
    }
}
