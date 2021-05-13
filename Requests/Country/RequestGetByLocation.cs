using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Country
{
    public class RequestGetByLocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
