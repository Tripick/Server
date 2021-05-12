using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class CountryPoint
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double x { get; set; }
        public double y { get; set; }

        public CountryPoint() { }

        public CountryPoint(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        [ForeignKey("Polygon")]
        public int PolygonId { get; set; }
        public CountryPolygon Polygon { get; set; }
    }
}
