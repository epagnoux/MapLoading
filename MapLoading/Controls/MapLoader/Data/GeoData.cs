using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MapLoading.Controls.MapLoader.Data
{
  class GeoData
  {
    public string City
    {
      get
      {
        //JObject o1 = JObject.Parse(File.ReadAllText(@"c:\videogames.json"));
        const string FILE_NAME = "WorldCityInfo.json";
        string wPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Assets\Data\", FILE_NAME);
        var wResult = string.Empty;

        //var wFile = File.OpenText(wPath);
        try
        {
          using (var wSr = new StreamReader(wPath))
          {
            wResult = wSr.ReadToEnd();
          }
        }
        catch (Exception ex)
        {
          new Exception("Could not read the file");
        }

        return wResult;
      }
    }

    public string Airport
    {
      get
      {
        //JObject o1 = JObject.Parse(File.ReadAllText(@"c:\videogames.json"));
        const string FILE_NAME = "WorldAirportInfo.json";
        string wPath = Path.Combine(Environment.CurrentDirectory, @"..\..\Assets\Data\", FILE_NAME);
        var wResult = string.Empty;

        //var wFile = File.OpenText(wPath);
        try
        {
          using (var wSr = new StreamReader(wPath))
          {
            wResult = wSr.ReadToEnd();
          }
        }
        catch (Exception ex)
        {
          new Exception("Could not read the file");
        }

        return wResult;
      }
    }
  }
}
