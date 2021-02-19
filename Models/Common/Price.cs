using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Price
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; }

        [ForeignKey("Currency")]
        public int IdCurrency { get; set; }
        public virtual Currency Currency { get; set; }
    }
}
