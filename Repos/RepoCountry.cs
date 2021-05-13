﻿using Microsoft.EntityFrameworkCore;
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

        public List<Country> GetAll(int quantity)
        {
            return this.TripickContext.Countries.Take(quantity).Include(c => c.Polygons).ThenInclude(p => p.Points.OrderBy(po => po.index)).ToList();
        }

        public List<Country> GetAllLight()
        {
            return this.TripickContext.Countries.ToList();
        }
    }
}
