using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Currency : ModelBase<Currency>
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Symbol { get; set; }
        public double ToUSDFactor { get; set; }

        public Currency ToDTO() { return this; }
    }
}
