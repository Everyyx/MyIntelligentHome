using MyIntelligentHomeSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Helpers
{
   public class MyWeatherHelper
    {
        public async static Task<WeatherRootObject> GetWeatherRootObjectAsync(string apikey,double lat,double lon)
        {
            HttpClient getWeaher = new HttpClient();
            var responce = await getWeaher.GetAsync($"http://api.jisuapi.com/weather/query?appkey={apikey}&location={lat},{lon}");
                var result = await responce.Content.ReadAsStringAsync();
                return await Myjson.ToObjectAsync<WeatherRootObject>(result);

                
        }
    }
}
