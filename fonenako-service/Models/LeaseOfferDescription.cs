
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using fonenako.Models;

namespace fonenako_service.Models
{
    [Table("LeaseOfferDescription")]
    public class LeaseOfferDescription
    {
        [Column("LeaseOfferDescriptionId")]
        public int LeaseOfferDescriptionId { get; set; }

        [Required]
        [Column("Content")]
        public string Content { get; set; }

        public LeaseOffer LeaseOffer { get; set; }

        public int LeaseOfferId { get; set; }
    }
}
