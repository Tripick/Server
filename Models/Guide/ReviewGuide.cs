using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class ReviewGuide
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual List<VoteReviewGuide> Votes { get; set; }

        [ForeignKey("Guide")]
        public int IdGuide { get; set; }
        public virtual Guide Guide { get; set; }

        [ForeignKey("Author")]
        public int IdAuthor { get; set; }
        public virtual AppUser Author { get; set; }
    }
}
