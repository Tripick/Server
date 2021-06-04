using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TripickServer.Models;

namespace TripickServer.Requests.Place
{
    public class RequestGenerationSaveImages
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public List<string> Urls { get; set; }
    }
}
