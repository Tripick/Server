using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripickServer.Models
{
    public class Flag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } = "Input";
        public string Icon { get; set; }
        public int MinVal { get; set; } = 0;
        public int MaxVal { get; set; } = 0;
        public int MaxLength { get; set; } = 0;
    }
}
