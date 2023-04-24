using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fonenako_service.Models;

namespace fonenako.Models
{
    [Table("LeaseOffer")]

    public class LeaseOffer
    {
        [Column("OfferId")]
        public int LeaseOfferID { get; set; }

        [Column("Title")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column("Surface")]
        public double Surface { get; set; }

        [Column("Rooms")]
        public int Rooms { get; set; }

        [Required]
        [Column("MonthlyRent")]
        public double MonthlyRent { get; set; }

        [Column("Carousel")]
        public string Carousel { get; set; } = string.Empty;

        [Column("Photos")]
        public string ConcatenedPhotos { get; set; } = string.Empty;

        [Column("CreationDate")]
        public DateTime CreationDate { get; set; }

        public int LocalisationId { get; set; }

        public Localisation Localisation { get; set; }

        public LeaseOfferDescription LeaseOfferDescription { get; set; }
    }
}