
using System.Text.Json.Serialization;

namespace fonenako_service
{
    public class CityFilter
    {
        public static readonly CityFilter Default = new ();

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
