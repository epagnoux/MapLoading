using Cae.Synapse.Foundation.Toolkit;
using Cae.Synapse.Foundation.Toolkit.Extensions;
using MapLoading.Helper;
using Newtonsoft.Json;

namespace MapLoading.Controls.MapLoader.Models
{
  public enum MarkerCategory
  {
    City,
    Airport,
  }

  public class MarkerBase : NotificationObject
  {
    private string mCity;
    private GeoCoordinate mCoordinate;

    private string mIso3;
    private double mLatitude;
    private double mLongitude;
    private double mPopulation;
    private string mProvince;

    public MarkerCategory Category { get; set; }

    [JsonProperty("iso3")]
    public string ISO3
    {
      get { return mIso3; }
      set
      {
        if (mIso3 == value) return;
        mIso3 = value;
        NotifyPropertyChanged(this.NameOf(p => p.ISO3));
      }
    }

    public string City
    {
      get { return mCity; }
      set
      {
        if (mCity == value) return;
        mCity = value;
        NotifyPropertyChanged(this.NameOf(p => p.City));
      }
    }


    [JsonProperty("lat")]
    public double Latitude
    {
      get { return mLatitude; }
      set
      {
        if (mLatitude == value) return;
        mLatitude = value;
        NotifyPropertyChanged(this.NameOf(p => p.Latitude));
      }
    }

    [JsonProperty("lng")]
    public double Longitude
    {
      get { return mLongitude; }
      set
      {
        if (mLongitude == value) return;
        mLongitude = value;
        NotifyPropertyChanged(this.NameOf(p => p.Longitude));
      }
    }

    [JsonProperty("province")]
    public string Province
    {
      get { return mProvince; }
      set
      {
        if (mProvince == value) return;
        mProvince = value;
        NotifyPropertyChanged(this.NameOf(p => p.Province));
      }
    }

    [JsonProperty("pop")]
    public double Population
    {
      get { return mPopulation; }
      set
      {
        if (mPopulation == value) return;
        mPopulation = value;
        NotifyPropertyChanged(this.NameOf(p => p.Population));
      }
    }

    //public GeoCoordinate Coordinate
    //{
    //  get { return mCoordinate; }
    //  set
    //  {
    //    if (mCoordinate.Equals(value)) return;
    //    mCoordinate = value;
    //    NotifyPropertyChanged(this.NameOf(p => p.Coordinate));
    //  }
    //}
  }
}