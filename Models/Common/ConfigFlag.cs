using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class ConfigFlag : ModelBase<ConfigFlag>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Type { get; set; } = "Input";
        public string ValType { get; set; } = "Double";
        public int MaxLength { get; set; } = 0;
        public int MinVal { get; set; } = 0;
        public int MaxVal { get; set; } = 0;
        public string Unit { get; set; } = "";

        public ConfigFlag ToDTO() { return this; }
    }
}
