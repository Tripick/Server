using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class ImageGuide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsVerified { get; set; }

        [ForeignKey("Guide")]
        public int IdGuide { get; set; }
        public virtual Guide Guide { get; set; }
    }
}
