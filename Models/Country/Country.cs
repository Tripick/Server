using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class Country : ModelBase<Country>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public List<CountryPolygon> Polygons { get; set; }
        public List<CountryArea> Areas { get; set; }

        public Country ToDTO()
        {
            return new Country()
            {
                Id = this.Id,
                Name = this.Name,
                Code = this.Code,
                Polygons = this.Polygons?.ToDTO(),
                Areas = this.Areas?.ToDTO(),
            };
        }
    }
}
