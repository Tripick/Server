using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;
using TripickServer.Utils;

namespace TripickServer.Models
{
    public class Day : ModelBase<Day>
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

        public Day ToDTO()
        {
            return new Day()
            {
                Id = this.Id,
                Name = this.Name,
                Description = this.Description,
                Note = this.Note,
                IdItinerary = this.IdItinerary,
                Itinerary = this.Itinerary?.ToDTO(),
                ToBrings = this.ToBrings?.ToDTO(),
                Steps = this.Steps?.ToDTO(),
            };
        }
    }
}
