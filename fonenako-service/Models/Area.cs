
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using fonenako.Models;

namespace fonenako_service.Models
{
    [Table("Area")]
    public class Area
    {
        [Column("AreaId")]
        public int AreaId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        public City City { get; set; }

        [Column("CityId")]
        public int CityId { get; set; }

        public ICollection<LeaseOffer> LeaseOffers { get; set; } = new HashSet<LeaseOffer>();
    }
}
