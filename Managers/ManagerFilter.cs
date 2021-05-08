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

namespace TripickServer.Managers
{
    public class ManagerFilter : ManagerBase
    {
        #region Properties

        private RepoTrip repoTrip;
        private RepoFilter repoFilter;

        #endregion

        #region Constructor

        public ManagerFilter(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoTrip = new RepoTrip(this.ConnectedUser, tripickContext);
            this.repoFilter = new RepoFilter(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public List<Filter> Get(int idTrip)
        {
            List<Filter> filters = get(idTrip);
            filters.ForEach(f => f.User = null);
            filters.ForEach(f => f.Trip = null);
            return filters;
        }

        public List<Filter> Save(int idTrip, List<Filter> filters)
        {
            Trip trip = this.repoTrip.GetById(idTrip);
            if(trip == null)
                throw new NullReferenceException("The trip does not exist.");

            // Get potential existing filters
            List<Filter> existings = get(idTrip);

            // Apply filters values
            foreach (Filter filter in filters)
            {
                Filter existing = existings.FirstOrDefault(f => f.Name == filter.Name);
                if (existing != null)
                {
                    existing.Min = filter.Min;
                    existing.Max = filter.Max;
                }
                else
                {
                    filter.IdUser = this.ConnectedUser().Id;
                    filter.IdTrip = idTrip;
                    this.repoFilter.Add(filter);
                }
            }

            // Commit
            this.TripickContext.SaveChanges();
            filters.ForEach(f => { f.User = null; f.Trip = null; });
            return filters;
        }

        #endregion

        #region Private

        private List<Filter> get(int idTrip)
        {
            List<Filter> filters =  this.repoFilter.Get(f => f.IdTrip == idTrip && f.IdUser == this.ConnectedUser().Id).ToList();
            if(!filters.Any())
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
            return filters;
        }

        #endregion
    }
}
