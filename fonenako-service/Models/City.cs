
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace fonenako_service.Models
{
    [Table("City")]
    public class City
    {
        [Column("CityId")]
        public int CityId { get; set; }

        [Column("Name")]
        public string Name { get; set; }

        public ICollection<Area> Areas { get; set; }
    }
}
