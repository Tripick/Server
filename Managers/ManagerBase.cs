using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripickServer.Models;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Managers
{
    public class ManagerBase
    {
        #region Properties

        protected readonly ILogger<ServerLogger> Logger;
        protected readonly Func<AppUser> ConnectedUser;
        protected readonly TripickContext TripickContext;
        

        #endregion

        #region Constructor

        public ManagerBase(ILogger<ServerLogger> logger, Func<AppUser> user, TripickContext tripickContext)
        {
            this.Logger = logger;
            this.ConnectedUser = user;
            this.TripickContext = tripickContext;
        }

        #endregion

        #region Public

        public JsonResult SafeCall<T>(Func<T> method) where T : ModelBase<T>, new()
        {
            try
            {
                T result = method();
                return ServerResponse<T>.ToJson(result == null ? result : result.ToDTO());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        public JsonResult SafeCall<T>(Func<List<T>> method) where T : ModelBase<T>, new()
        {
            try
            {
                List<T> result = method();
                return ServerResponse<List<T>>.ToJson(result == null ? result : result.ToDTO());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        public JsonResult SafeCallValueType<T>(Func<T> method)
        {
            try
            {
                return ServerResponse<T>.ToJson(method());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        public async Task<JsonResult> SafeCallAsync<T>(Func<Task<T>> method) where T : ModelBase<T>, new()
        {
            try
            {
                T result = await method();
                return ServerResponse<T>.ToJson(result == null ? result : result.ToDTO());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        public async Task<JsonResult> SafeCallAsync<T>(Func<Task<List<T>>> method) where T : ModelBase<T>, new()
        {
            try
            {
                List<T> result = await method();
                return ServerResponse<List<T>>.ToJson(result == null ? result : result.ToDTO());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        public async Task<JsonResult> SafeCallValueTypeAsync<T>(Func<Task<T>> method)
        {
            try
            {
                return ServerResponse<T>.ToJson(await method());
            }
            catch (Exception e)
            {
                return ServerResponse<T>.ToJson(false, e.Message);
            }
        }

        public AppConfiguration LoadConfiguration()
        {
            return new AppConfiguration()
            {
                Flags = this.TripickContext.ConfigReviewFlags.ToList()
            };
        }

        #endregion
    }
}
