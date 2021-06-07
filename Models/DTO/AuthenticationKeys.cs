using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class AuthenticationKeys : ModelBase<AuthenticationKeys>
    {
        public int Id { get; set; }
        public string AccessToken { get; set; }

        public AuthenticationKeys ToDTO() { return this; }
    }
}
