using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class ImageAppUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("Owner")]
        public int IdOwner { get; set; }
        public virtual AppUser Owner { get; set; }
    }
}
