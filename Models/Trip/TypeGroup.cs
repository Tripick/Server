using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class TypeGroup : ModelBase<TypeGroup>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int NbPersons { get; set; }
        public string Icon { get; set; }

        public TypeGroup ToDTO() { return this; }
    }
}
