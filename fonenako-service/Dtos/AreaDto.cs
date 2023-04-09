
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace fonenako_service.Dtos
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class AreaDto
    {
        [JsonPropertyName("areaId")]
        public int AreaId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("city")]
        public CityDto City { get; set; }
    }
}
