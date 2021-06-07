using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TripickServer.Models.Common;

namespace TripickServer.Models
{
    public class Step : ModelBase<Step>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string CoverImage { get; set; }
        public string DistanceFromPrevious { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool IsActive { get; set; }
        public bool IsCustom { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [ForeignKey("Day")]
        public int IdDay { get; set; }
        public virtual Day Day { get; set; }

        [ForeignKey("Pick")]
        public int? IdPick { get; set; }
        public virtual Pick Pick { get; set; }

        [ForeignKey("Type")]
        public int? IdType { get; set; }
        public virtual TypeStep Type { get; set; }

        public Step ToDTO()
        {
            return new Step()
            {
                Id = this.Id,
                IdDay = this.IdDay,
                IdPick = this.IdPick,
                IdType = this.IdType,
                Name = this.Name,
                Description = this.Description,
                Note = this.Note,
                CoverImage = this.CoverImage,
                DistanceFromPrevious = this.DistanceFromPrevious,
                Start = this.Start,
                End = this.End,
                IsActive = this.IsActive,
                IsCustom = this.IsCustom,
                Latitude = this.Latitude,
                Longitude = this.Longitude,
                Pick = this.Pick.ToDTO(),
                Type = this.Type.ToDTO()
            };
        }
    }
}
