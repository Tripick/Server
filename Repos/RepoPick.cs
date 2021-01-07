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

        public List<Pick> GetAllByTrip(int idTrip)
        {
            List<Pick> picks = this.TripickContext.Picks.Where(p => p.IdUser == this.ConnectedUser().Id && p.IdTrip == idTrip).ToList();
            return picks;
        }
    }
}
