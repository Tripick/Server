using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Configuration
    {
        [Key]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
