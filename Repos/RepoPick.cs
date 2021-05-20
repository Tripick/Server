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

        public List<Pick> GetAllByTrip(int idTrip)
        {
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip).ToList();
        }

        public Pick Find(int idTrip, int idPlace)
        {
            return this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip && p.IdPlace == idPlace).FirstOrDefault();
        }
    }
}
