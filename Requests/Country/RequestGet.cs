using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Country
{
    public class RequestGet
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
