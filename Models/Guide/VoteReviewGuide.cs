using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class VoteReviewGuide : ModelBase<VoteReviewGuide>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool IsUp { get; set; }

        [ForeignKey("ReviewGuide")]
        public int IdReviewGuide { get; set; }
        public virtual ReviewGuide ReviewGuide { get; set; }

        [ForeignKey("Author")]
        public int IdAuthor { get; set; }
        public virtual AppUser Author { get; set; }

        public VoteReviewGuide ToDTO()
        {
            return new VoteReviewGuide()
            {
                Id = this.Id,
                IsUp = this.IsUp,
                IdReviewGuide = this.IdReviewGuide,
                IdAuthor = this.IdAuthor
            };
        }
    }
}
