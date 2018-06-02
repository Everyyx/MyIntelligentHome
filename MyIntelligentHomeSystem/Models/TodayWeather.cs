using MyIntelligentHomeSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Models
{
    public class TodayWeather:WeatherViewModel
    {
        private string _cityname;   //天津
        private string _temphigh;   //40
        private string _templow;    //9
        private string _weatherdescription;     //多云
        private string _lastupdatetime;         //2018-4-19 15：00：24
        private string _airqualityindex;        //112
        private string _aqilevel;               //轻度污染
        private string _icon;                   //Assets/weather/XXX.png(jpg)
        private string _temp;
        private string _aqicolor;
        private string _humidity;

        public string CityName
        {
            get { return _cityname; }
            set
            {
                this._cityname = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string TempHigh
        {
            get { return _temphigh; }
            set
            {
                this._temphigh = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string TempLow
        {
            get { return _templow; }
            set
            {
                this._templow = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string WeatherDescription
        {
            get { return _weatherdescription; }
            set
            {
                this._weatherdescription = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string LastUpdateTime
        {
            get { return _lastupdatetime; }
            set
            {
                this._lastupdatetime = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string AirQualityIndex
        {
            get { return _airqualityindex; }
            set
            {
                this._airqualityindex = value;
                WeatherRaisePropertyChanged();         
            }
        }
        public string AqiLevel
        {
            get { return _aqilevel; }
            set
            {
                this._aqilevel = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string Icon
        {
            get { return _icon; }
            set
            {
                this._icon = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string Temp
        {
            get { return _temp; }
            set
            {
                this._temp = value;
                WeatherRaisePropertyChanged();
            }
        }
        public string AqiColor
        {
            get { return _aqicolor; }
            set
            {
                this._aqicolor = value;
                WeatherRaisePropertyChanged();
            }
        }

        public string Humidity
        {
            get { return _humidity; }
            set
            {
                this._humidity = value;
                WeatherRaisePropertyChanged();
            }
        }
    }
}
