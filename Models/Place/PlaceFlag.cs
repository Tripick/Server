using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class PlaceFlag : ModelBase<PlaceFlag>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Value { get; set; }

        [ForeignKey("Config")]
        public int IdConfig { get; set; }
        public virtual ConfigFlag Config { get; set; }

        [ForeignKey("Place")]
        public int IdPlace { get; set; }
        public virtual Place Place { get; set; }

        public PlaceFlag ToDTO()
        {
            return new PlaceFlag()
            {
                Id = this.Id,
                IdPlace = this.IdPlace,
                IdConfig = this.IdConfig,
                Config = this.Config,
                Value = this.Value,
            };
        }
    }
}
