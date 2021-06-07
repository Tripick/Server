using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Price : ModelBase<Price>
    {
        [Key]
        public int Id { get; set; }
        public double Value { get; set; }

        [ForeignKey("Currency")]
        public int IdCurrency { get; set; }
        public virtual Currency Currency { get; set; }

        public Price ToDTO()
        {
            return new Price()
            {
                Id = this.Id,
                Value = this.Value,
                IdCurrency = this.IdCurrency,
                Currency = this.Currency?.ToDTO()
            };
        }
    }
}
