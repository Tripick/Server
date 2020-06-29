using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Day
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }

        [ForeignKey("Itinerary")]
        public int IdItinerary { get; set; }
        public virtual Itinerary Itinerary { get; set; }

        public virtual List<ToBring> ToBrings { get; set; }
        public virtual List<Step> Steps { get; set; }
    }
}
