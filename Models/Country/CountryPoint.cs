using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class CountryPoint : ModelBase<CountryPoint>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double index { get; set; }

        public CountryPoint() { }

        public CountryPoint(double x, double y, double index)
        {
            this.x = x;
            this.y = y;
            this.index = index;
        }

        [ForeignKey("Polygon")]
        public int PolygonId { get; set; }
        public CountryPolygon Polygon { get; set; }

        public CountryPoint ToDTO()
        {
            return this;
        }
    }
}
