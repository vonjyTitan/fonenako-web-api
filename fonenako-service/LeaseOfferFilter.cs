
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace fonenako_service
{
    public class LeaseOfferFilter
    {
        public static readonly LeaseOfferFilter Default = new();

        [JsonPropertyName("rooms")]
        public int? Rooms { get; set; }

        [JsonPropertyName("monthlyRentMin")]
        public double? MonthlyRentMin { get; set; }

        [JsonPropertyName("monthlyRentMax")]
        public double? MonthlyRentMax { get; set; }

        [JsonPropertyName("surfaceMin")]
        public double? SurfaceMin { get; set; }

        [JsonPropertyName("surfaceMax")]
        public double? SurfaceMax { get; set; }

        public override bool Equals(object obj)
        {
            return obj is LeaseOfferFilter filter &&
                   Rooms == filter.Rooms &&
                   MonthlyRentMin == filter.MonthlyRentMin &&
                   MonthlyRentMax == filter.MonthlyRentMax &&
                   SurfaceMin == filter.SurfaceMin &&
                   SurfaceMax == filter.SurfaceMax;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Rooms, MonthlyRentMin, MonthlyRentMax, SurfaceMin, SurfaceMax);
        }

        public static bool operator ==(LeaseOfferFilter left, LeaseOfferFilter right)
        {
            return EqualityComparer<LeaseOfferFilter>.Default.Equals(left, right);
        }

        public static bool operator !=(LeaseOfferFilter left, LeaseOfferFilter right)
        {
            return !(left == right);
        }
    }
}
