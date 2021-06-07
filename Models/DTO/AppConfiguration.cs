using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class AppConfiguration : ModelBase<AppConfiguration>
    {
        public List<ConfigFlag> Flags { get; set; }

        public AppConfiguration ToDTO()
        {
            return new AppConfiguration()
            {
                Flags = this.Flags?.ToDTO()
            };
        }
    }
}
