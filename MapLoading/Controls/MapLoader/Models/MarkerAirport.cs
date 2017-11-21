using Cae.Synapse.Foundation.Toolkit.Extensions;
using Newtonsoft.Json;

namespace MapLoading.Controls.MapLoader.Models
{
  public class MarkerAirport : MarkerBase
  {
    private int mDirectFlights;
    private string mICAO;
    private string mSationType;

    public MarkerAirport()
    {
      Category = MarkerCategory.Airport;
    }
    public string ICAO
    {
      get { return mICAO; }
      set
      {
        if (mICAO == value) return;
        mICAO = value;
        NotifyPropertyChanged(this.NameOf(p => p.ICAO));
      }
    }

    [JsonProperty("direct_flights")]
    public int DirectFlights
    {
      get { return mDirectFlights; }
      set
      {
        if (mDirectFlights == value) return;
        mDirectFlights = value;
        NotifyPropertyChanged(this.NameOf(p => p.DirectFlights));
      }
    }

    [JsonProperty("type")]
    public string SationType
    {
      get { return mSationType; }
      set
      {
        if (mSationType == value) return;
        mSationType = value;
        NotifyPropertyChanged(this.NameOf(p => p.SationType));
      }
    }
  }
}