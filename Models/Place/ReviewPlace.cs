﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class ReviewPlace
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime CreationDate { get; set; }
        public virtual List<VoteReviewPlace> Votes { get; set; }

        [ForeignKey("Place")]
        public int IdPlace { get; set; }
        public virtual Place Place { get; set; }

        [ForeignKey("Author")]
        public int IdAuthor { get; set; }
        public virtual AppUser Author { get; set; }
    }
}