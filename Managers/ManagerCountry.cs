﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TripickServer.Models;
using TripickServer.Repos;
using TripickServer.Utils;

namespace TripickServer.Managers
{
    public class ManagerCountry : ManagerBase
    {
        #region Properties

        private RepoCountry repoCountry;
        private RepoCountryArea repoCountryAreas;

        #endregion

        #region Constructor

        public ManagerCountry(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext) : base(logger, user, tripickContext)
        {
            this.repoCountry = new RepoCountry(this.ConnectedUser, tripickContext);
            this.repoCountryAreas = new RepoCountryArea(this.ConnectedUser, tripickContext);
        }

        #endregion

        #region Public

        public Country Get(int? id, string name)
        {
            return id.HasValue ? getById(id.Value) : getByName(name);
        }

        public Country GetComplete(int id)
        {
            return this.repoCountry.GetComplete(id);
        }

        public List<Country> GetByLocation(double latitude, double longitude)
        {
            return this.repoCountry.GetByLocation(latitude, longitude);
        }

        public List<Country> GetAll()
        {
            return this.repoCountry.GetAll();
        }

        public List<Country> GenerationGetAll(int quantity)
        {
            return this.repoCountry.GenerationGetAll(quantity);
        }

        public bool GenerationSave(Country country)
        {
            Country existingCountry = this.repoCountry.GetComplete(country.Id);
            if (existingCountry != null)
            {
                existingCountry.Areas.AddRange(country.Areas.Select(area => new CountryArea()
                {
                    MinLat = area.MinLat,
                    MinLon = area.MinLon,
                    MaxLat = area.MaxLat,
                    MaxLon = area.MaxLon
                }).ToList());

                // Commit
                this.TripickContext.SaveChanges();
            }
            return true;
        }

        #endregion

        #region Private

        private Country getById(int id)
        {
            return this.repoCountry.GetById(id);
        }

        private Country getByName(string name)
        {
            return this.repoCountry.Get(c => c.Name == name).FirstOrDefault();
        }

        #endregion
    }
}