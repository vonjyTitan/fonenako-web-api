
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace fonenako_service.Dtos
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class CityDto
    {
        [JsonPropertyName("cityId")]
        public int CityId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public ICollection<AreaDto> Areas { get; set; } = new HashSet<AreaDto>(0);
    }
}
