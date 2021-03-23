using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoFilter : RepoCRUD<Filter>
    {
        #region Constructor

        public RepoFilter(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<Filter> GetAllForTrip(int idTrip)
        {
            List<Filter> filters = this.TripickContext.Filters.Where(f => f.IdUser == this.ConnectedUser().Id && f.IdTrip == idTrip).ToList();
            return filters;
        }
    }
}
