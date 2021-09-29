using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Retribution : ModelBase<Retribution>
    {
        [Key]
        public int Id { get; set; }
        public string ActionName { get; set; }
        public string Data { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
        public virtual AppUser User { get; set; }

        public Retribution ToDTO() { return new Retribution() { Id = this.Id, ActionName = this.ActionName, Data = this.Data, IdUser = this.IdUser }; }
    }
}
