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
            if (!filters.Any())
            {
                filters = new List<Filter>()
                {
                    new Filter() { Name="Price", Min=0, Max=99 },
                    new Filter() { Name = "Length", Min = 0, Max = 300 },
                    new Filter() { Name = "Duration", Min = 0, Max = 999 },
                    new Filter() { Name = "Difficulty", Min = 0, Max = 5 },
                    new Filter() { Name = "Touristy", Min = 0, Max = 5 }
                };
            }
            else
            {
                filters.ForEach(f => { f.Trip = null; f.User = null; });
            }
            return filters;
        }
    }
}
