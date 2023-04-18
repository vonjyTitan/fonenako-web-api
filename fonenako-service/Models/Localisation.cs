
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using fonenako.Models;

namespace fonenako_service.Models
{
    [Table("Localisation")]
    public class Localisation
    {
        [Column("LocalisationId")]
        public int LocalisationId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        [Column("Type")]
        public LocalisationType Type { get; set; }

        public Localisation Hierarchy { get; set; }

        public ICollection<LeaseOffer> LeaseOffers { get; set; } = new HashSet<LeaseOffer>();

        public ICollection<Localisation> SubLocalisations { get; set; } = new HashSet<Localisation>();
    }
}
