using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoItineraryDay : RepoCRUD<ItineraryDay>
    {
        #region Constructor

        public RepoItineraryDay(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public ItineraryDay GetWithSteps(int idDay)
        {
            ItineraryDay day = this.TripickContext.ItineraryDays.FirstOrDefault(d => d.Id == idDay);
            day.Steps = this.TripickContext.ItineraryDaySteps
                .Where(s => s.IdDay == idDay)
                .Include(s => s.Visit)
                .ThenInclude(v => v.Place)
                .OrderBy(s => s.Index)
                .ToList();
            return day;
        }
    }
}
