using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;

namespace TripickServer.Repos
{
    public class RepoCountry : RepoCRUD<Country>
    {
        #region Constructor

        public RepoCountry(Func<AppUser> connectedUser, TripickContext tripickContext) : base(connectedUser, tripickContext) { }

        #endregion

        public Country GetCountryOfPosition(double latitude, double longitude)
        {
            List<Country> countries = this.TripickContext.Countries.Include(c => c.Polygons).ThenInclude(p => p.Points).ToList();
            Country country = countries.Where(c => c.Polygons.Any(p => isPointInPolygon(p.Points, latitude, longitude))).FirstOrDefault();
            if (country == null)
                return new Country() { Code = null, Name = "" };
            return new Country() { Id=country.Id, Code=country.Code, Name=country.Name };
        }

        public List<Country> GetAll()
        {
            return this.TripickContext.Countries.Include(c => c.Polygons).ThenInclude(p => p.Points).ToList();
        }

        private bool isPointInPolygon(List<CountryPoint> polygon, double latitude, double longitude)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].y < longitude && polygon[j].y >= longitude || polygon[j].y < longitude && polygon[i].y >= longitude)
                {
                    if (polygon[i].x + (longitude - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < latitude)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }
    }
}
