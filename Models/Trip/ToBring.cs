using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class ToBring : ModelBase<ToBring>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }

        [ForeignKey("Type")]
        public int? IdType { get; set; }
        public virtual TypeToBring Type { get; set; }

        public ToBring ToDTO()
        {
            return new ToBring()
            {
                Id = this.Id,
                Name = this.Name,
                Checked = this.Checked,
                IdType = this.IdType,
                Type = this.Type?.ToDTO()
            };
        }
    }
}
