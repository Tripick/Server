using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoPick : RepoCRUD<Pick>
    {
        #region Constructor

        public RepoPick(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public int CountAllByTrip(int idTrip)
        {
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip).Count();
        }

        public int CountNotZeroByTrip(int idTrip)
        {
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip && p.Rating > 0).Count();
        }

        public List<Pick> GetAll(int idTrip, int quantity, int skip)
        {
            List<Pick> picks = this.TripickContext.Picks
                .Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip)
                .Skip(skip)
                .Take(quantity)
                .Include(p => p.Place).ThenInclude(p => p.Images)
                .Include(p => p.Place).ThenInclude(p => p.Reviews)
                .Include(p => p.Place).ThenInclude(p => p.Flags)
                .ToList();
            return picks;
        }

        public List<Pick> GetAllByTrip(int idTrip, int? minRating = null)
        {
            if(minRating.HasValue)
                return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip && p.Rating > minRating.Value).ToList();
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip).ToList();
        }

        public List<Pick> GetPicked(int idTrip)
        {
            return this.TripickContext.Picks
                .Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip && p.Rating > 0)
                .Include(p => p.Place)
                .ToList();
        }

        public Pick Find(int idTrip, int idPlace)
        {
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip && p.IdPlace == idPlace).FirstOrDefault();
        }

        public Pick GetByIdPlace(int idTrip, int idPlace)
        {
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip && p.IdPlace == idPlace).FirstOrDefault();
        }
    }
}
