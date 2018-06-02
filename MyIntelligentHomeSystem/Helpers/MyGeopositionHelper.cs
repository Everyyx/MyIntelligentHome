using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace MyIntelligentHomeSystem.Helpers
{
    public class MyGeopositionHelper
    {
        public static  async Task<Geoposition> GetGeopositionasync()
        {
            Geolocator mygeolocator = new Geolocator();
            Geoposition geoposition = await mygeolocator.GetGeopositionAsync(); //6,467ms
            return geoposition;
        }

    }
}
