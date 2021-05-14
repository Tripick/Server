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

        public List<Country> GetByLocation(double latitude, double longitude)
        {
            List<CountryArea> areas = this.TripickContext.CountryAreas.Where(a =>
                a.MinLat <= latitude && latitude <= a.MaxLat && a.MinLon <= longitude && longitude <= a.MaxLon
                ).Include(a => a.Country).ToList();

            return areas.Select(area => { return new Country() { Id = area.Country.Id, Code = area.Country.Code, Name = area.Country.Name }; }).ToList();
        }

        public Country GetComplete(int id)
        {
            return this.TripickContext.Countries.Where(c => c.Id == id).Include(a => a.Areas).FirstOrDefault();
        }

        public List<Country> GenerationGetAll(int quantity)
        {
            // TODO Hugo : remove the WHERE clause here !!!!!!!!!
            return this.TripickContext.Countries.Where(c => !c.Areas.Any()).Take(quantity).Include(c => c.Polygons).ThenInclude(p => p.Points.OrderBy(po => po.index)).ToList();
        }

        public List<Country> GetAll()
        {
            return this.TripickContext.Countries.OrderBy(c => c.Name).ToList();
        }
    }
}
