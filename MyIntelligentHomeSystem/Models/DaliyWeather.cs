using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Models
{
    public class DaliyWeather
    {
        public string Date { get; set; }
        public string Img { get; set; }
        public string TempHigh { get; set; }
        public string TempLow { get; set; }
        public string Weatherdescription { get; set; }

        public string AqiColor { get; set; }

        public static List<DaliyWeather> GetWeathers(WeatherRootObject root)
        {
            var weathers = new List<DaliyWeather>
                {
                    new DaliyWeather { Date = ""+root.result.daily[0].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[0].day.img}.png"), TempHigh ="↑："+ root.result.daily[0].day.temphigh+"°", TempLow ="↓："+root.result.daily[0].night.templow+"°", Weatherdescription = root.result.daily[0].day.weather ,AqiColor=root.result.aqi.aqiinfo.color},
                    new DaliyWeather { Date = ""+root.result.daily[1].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[1].day.img}.png"), TempHigh ="↑："+ root.result.daily[1].day.temphigh+"°", TempLow ="↓："+root.result.daily[1].night.templow+"°", Weatherdescription = root.result.daily[1].day.weather ,AqiColor=root.result.aqi.aqiinfo.color},
                    new DaliyWeather { Date = ""+root.result.daily[2].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[2].day.img}.png"), TempHigh ="↑："+ root.result.daily[2].day.temphigh+"°", TempLow ="↓："+root.result.daily[2].night.templow+"°", Weatherdescription = root.result.daily[2].day.weather ,AqiColor=root.result.aqi.aqiinfo.color},
                    new DaliyWeather { Date = ""+root.result.daily[3].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[3].day.img}.png"), TempHigh ="↑："+ root.result.daily[3].day.temphigh+"°", TempLow ="↓："+root.result.daily[3].night.templow+"°", Weatherdescription = root.result.daily[3].day.weather ,AqiColor=root.result.aqi.aqiinfo.color},
                    new DaliyWeather { Date = ""+root.result.daily[4].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[4].day.img}.png"), TempHigh ="↑："+ root.result.daily[4].day.temphigh+"°", TempLow ="↓："+root.result.daily[4].night.templow+"°", Weatherdescription = root.result.daily[4].day.weather ,AqiColor=root.result.aqi.aqiinfo.color},
                    new DaliyWeather { Date = ""+root.result.daily[5].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[5].day.img}.png"), TempHigh ="↑："+ root.result.daily[5].day.temphigh+"°", TempLow ="↓："+root.result.daily[5].night.templow+"°", Weatherdescription = root.result.daily[5].day.weather ,AqiColor=root.result.aqi.aqiinfo.color},
                    new DaliyWeather { Date = ""+root.result.daily[6].date, Img = string.Format($"ms-appx:///Assets/weathercn02/{root.result.daily[6].day.img}.png"), TempHigh ="↑："+ root.result.daily[6].day.temphigh+"°", TempLow ="↓："+root.result.daily[6].night.templow+"°", Weatherdescription = root.result.daily[6].day.weather ,AqiColor=root.result.aqi.aqiinfo.color}
                };
            return weathers;
        }

    }

}
