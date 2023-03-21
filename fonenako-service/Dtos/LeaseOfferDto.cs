using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace fonenako_service.Dtos
{
    [Serializable]
    public class LeaseOfferDto
    {
        [JsonPropertyName(LeaseOfferDtoProperties.LeaseOfferID)]
        public int LeaseOfferID { get; set; }

        [JsonPropertyName(LeaseOfferDtoProperties.Title)]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName(LeaseOfferDtoProperties.Surface)]
        public double Surface { get; set; }

        [JsonPropertyName("rooms")]
        public int Rooms { get; set; }

        [JsonPropertyName(LeaseOfferDtoProperties.MonthlyRent)]
        public double MonthlyRent { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("carouselUri")]
        public string CarouselUri { get; set; } = string.Empty;

        [JsonPropertyName("photoUris")]
        public IEnumerable<string> PhotoUris { get; set; } = Array.Empty<string>();
    }
}