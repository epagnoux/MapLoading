using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapLoading.Helper
{
  public struct GeoCoordinate
  {
    private readonly double mLatitude;
    private readonly double mLongitude;

    [JsonProperty("lat")]
    double Latitude { get; set; }
    [JsonProperty("lng")]
    double Longitude { get; set; }

    //public GeoCoordinate(double latitude, double longitude)
    //{
    //  Latitude = latitude;
    //  Longitude = longitude;
    //}

    public override string ToString()
    {
      return string.Format("{0},{1}", Latitude, Longitude);
    }
    public override bool Equals(Object other)
    {
      return other is GeoCoordinate && Equals((GeoCoordinate)other);
    }

    public bool Equals(GeoCoordinate other)
    {
      return Latitude == other.Latitude && Longitude == other.Longitude;
    }

    public override int GetHashCode()
    {
      return Latitude.GetHashCode() ^ Longitude.GetHashCode();
    }
  }
}
