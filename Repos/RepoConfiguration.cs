using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoConfiguration : RepoBase
    {
        #region Constructor

        public RepoConfiguration(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public List<Configuration> GetAll()
        {
            return this.TripickContext.Configurations.ToList();
        }

        public Configuration Get(string name)
        {
            return this.TripickContext.Configurations
                .Where(t => t.Name == name)
                .FirstOrDefault();
        }
    }
}
