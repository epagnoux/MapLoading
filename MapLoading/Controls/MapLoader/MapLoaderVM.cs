// =====================================================================================================================
//  Avis de propriété:
//  L'information ci-incluse est confidentielle et/ou appartient à CAE Inc. Elle ne peut être reproduite ou divulguée,
//  en tout ou en partie, et ne peut être utilisée que selon les termes du contrat de licence conclu avec CAE Inc.
//  Copyright CAE Inc., 2017. Tous droits réservés.
// 
//  Proprietary notice:
//  The information contained herein is confidential and/or proprietary to CAE Inc. It shall not be reproduced or
//  disclosed in whole or in part, and may only be used in accordance with the terms of the License Agreement entered
//  into with CAE Inc.
//  Copyright CAE Inc., 2017. All Rights Reserved.
// =====================================================================================================================

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Cae.Synapse.Foundation.Toolkit;
using Cae.Synapse.Foundation.Toolkit.Extensions;
using MapLoading.Controls.MapLoader.Data;
using MapLoading.Controls.MapLoader.Models;
using MapLoading.Helper;
using Newtonsoft.Json;

namespace MapLoading.Controls.MapLoader
{
  public class MapLoaderVM : NotificationObject
  {
    private ObservableCollection<MarkerCity> mMarkersCity;
    private ObservableCollection<MarkerAirport> mMarkersAirport;
    private IEnumerable<MarkerCity> mSelectedMarkersCity;
    private IEnumerable<MarkerAirport> mSelectedMarkersAirport;
    

    public MapLoaderVM()
    {

      Task.Run(() => FillData());
      //FillData();
    }

    private void FillData()
    {
      //GeoData = new ObservableCollection<MarkerCity>
      //{
      //  new MarkerCity {Airport = "FRA", City = "Paris", Coordinate = new GeoCoordinate(48.856614, 2.3522219000000177)},
      //  new MarkerCity {Airport = "JPN", City = "Tokyo", Coordinate = new GeoCoordinate(35.6894875, 139.69170639999993)},
      //  new MarkerCity {Airport = "MEX", City = "Cancun", Coordinate = new GeoCoordinate(21.161908, -86.85152790000001)},
      //  new MarkerCity {Airport = "DOM", City = "Punta Cana", Coordinate = new GeoCoordinate(18.5820101, -68.4054729)},
      //  new MarkerCity {Airport = "ZAF", City = "Le Cap", Coordinate = new GeoCoordinate(-33.9248685, 18.424055299999964)},
      //  new MarkerCity {Airport = "ISL", City = "Ísafjörður", Coordinate = new GeoCoordinate(66.0611061, -23.188859800000046)},
      //  new MarkerCity {Airport = "CYUL", City = "Montreal", Coordinate = new GeoCoordinate(45.4626804650683, -73.7658533104186)},
      //}; 
      
      var wCity = new GeoData().City;
      MarkersCity = JsonConvert.DeserializeObject<ObservableCollection<MarkerCity>>(wCity);
      //SelectedMarkersCity = MarkersCity;
      SelectedMarkersCity = MarkersCity.Where(p => p.Population > 2500000);
      //SelectedMarkersCity = MarkersCity.Where(p => p.City == "Montréal");
      //SelectedMarkersCity = MarkersCity;
      
      var wAirport = new GeoData().Airport;
      MarkersAirport = JsonConvert.DeserializeObject<ObservableCollection<MarkerAirport>>(wAirport);

      SelectedMarkersAirport = MarkersAirport.Where(p => p.ICAO != string.Empty);
      //var wResult = MarkersAirport.Where(p => p.DirectFlights >= 20);
      //var wResult = MarkersAirport.Where(p => p.ICAO == "CYUL");
      //SelectedMarkersAirport = wResult;
      //SelectedMarkersAirport = wResult;
    }

    public ObservableCollection<MarkerCity> MarkersCity
    {
      get { return mMarkersCity; }
      set
      {
        if (mMarkersCity == value) return;
        mMarkersCity = value;
        NotifyPropertyChanged(this.NameOf(p => p.MarkersCity));
      }
    }
    public ObservableCollection<MarkerAirport> MarkersAirport
    {
      get { return mMarkersAirport; }
      set
      {
        if (mMarkersAirport == value) return;
        mMarkersAirport = value;
        NotifyPropertyChanged(this.NameOf(p => p.MarkersAirport));
      }
    }

    public IEnumerable<MarkerCity> SelectedMarkersCity
    {
      get { return mSelectedMarkersCity; }
      set
      {
        if (mSelectedMarkersCity == value) return;
        mSelectedMarkersCity = value;
        NotifyPropertyChanged(this.NameOf(p => p.SelectedMarkersCity));
      }
    } 
    public IEnumerable<MarkerAirport> SelectedMarkersAirport
    {
      get { return mSelectedMarkersAirport; }
      set
      {
        if (mSelectedMarkersAirport == value) return;
        mSelectedMarkersAirport = value;
        NotifyPropertyChanged(this.NameOf(p => p.SelectedMarkersAirport));
      }
    }

    private IEnumerable<MarkerCity> GetMarkersBy(string key, MarkerCategory markerCategory)
    {
      switch (markerCategory)
      {
        case MarkerCategory.City:
          return MarkersCity.Where(p => p.City == key);

        case MarkerCategory.Airport:
          return MarkersCity.Where(p => p.ISO3 == key);
      }
      return null;
    }
  }
}