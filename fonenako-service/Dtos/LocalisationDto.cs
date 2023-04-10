﻿
using System.Text.Json.Serialization;
using fonenako_service.Models;
using Newtonsoft.Json;
using JsonConverterAttribute = System.Text.Json.Serialization.JsonConverterAttribute;

namespace fonenako_service.Dtos
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LocalisationDto
    {
        [JsonPropertyName("localisationId")]
        public int LocalisationId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("hierarchy")]
        public LocalisationDto Hierarchy { get; set; }

        [JsonPropertyName("type")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LocalisationType Type { get; set; }
    }
}
