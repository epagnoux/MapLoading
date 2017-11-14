using Cae.Synapse.Foundation.Toolkit;
using Cae.Synapse.Foundation.Toolkit.Extensions;
using MapLoading.Helper;
using Newtonsoft.Json;

namespace MapLoading.Controls.MapLoader.Models
{
  public class MarkerCity : MarkerBase
  {
    public MarkerCity()
    {
      Category = MarkerCategory.City;
    }
  }
}