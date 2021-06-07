using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class ImageAppUser : ModelBase<ImageAppUser>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Image { get; set; }
        public DateTime CreationDate { get; set; }

        [ForeignKey("Owner")]
        public int IdOwner { get; set; }
        public virtual AppUser Owner { get; set; }

        public ImageAppUser ToDTO()
        {
            return new ImageAppUser()
            {
                Id = this.Id,
                Image = this.Image,
                CreationDate = this.CreationDate,
                IdOwner = this.IdOwner
            };
        }
    }
}
