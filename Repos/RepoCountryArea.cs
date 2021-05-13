using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoCountryArea : RepoCRUD<CountryArea>
    {
        #region Constructor

        public RepoCountryArea(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

    }
}
