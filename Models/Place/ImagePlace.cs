using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class ImagePlace : ModelBase<ImagePlace>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Url { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsVerified { get; set; }
        public bool IsCover { get; set; }

        [ForeignKey("Place")]
        public int IdPlace { get; set; }
        public virtual Place Place { get; set; }

        [ForeignKey("Uploader")]
        public int IdUploader { get; set; }
        public virtual AppUser Uploader { get; set; }

        public ImagePlace ToDTO()
        {
            return new ImagePlace()
            {
                Id = this.Id,
                Url = this.Url,
                IdPlace = this.IdPlace,
                IdUploader = this.IdUploader
            };
        }
    }
}
