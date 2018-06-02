using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Models
{
    [DataContract]
    public class Index
    {
        [DataMember]
        public string iname { get; set; }
        [DataMember]
        public string ivalue { get; set; }
        [DataMember]
        public string detail { get; set; }
    }

    [DataContract]
    public class Aqiinfo
    {
        [DataMember]
        public string level { get; set; }
        [DataMember]
        public string color { get; set; }
        [DataMember]
        public string affect { get; set; }
        [DataMember]
        public string measure { get; set; }
    }
    [DataContract]

    public class Aqi
    {
        [DataMember]
        public string so2 { get; set; }
        [DataMember]
        public string so224 { get; set; }
        [DataMember]
        public string no2 { get; set; }
        [DataMember]
        public string no224 { get; set; }
        [DataMember]
        public string co { get; set; }
        [DataMember]
        public string co24 { get; set; }
        [DataMember]
        public string o3 { get; set; }
        [DataMember]
        public string o38 { get; set; }
        [DataMember]
        public string o324 { get; set; }
        [DataMember]
        public string pm10 { get; set; }
        [DataMember]
        public string pm1024 { get; set; }
        [DataMember]
        public string pm2_5 { get; set; }
        [DataMember]
        public string pm2_524 { get; set; }
        [DataMember]
        public string iso2 { get; set; }
        [DataMember]
        public string ino2 { get; set; }
        [DataMember]
        public string ico { get; set; }
        [DataMember]
        public string io3 { get; set; }
        [DataMember]
        public string io38 { get; set; }
        [DataMember]
        public string ipm10 { get; set; }
        [DataMember]
        public string ipm2_5 { get; set; }
        [DataMember]
        public string aqi { get; set; }
        [DataMember]
        public string primarypollutant { get; set; }
        [DataMember]
        public string quality { get; set; }
        [DataMember]
        public string timepoint { get; set; }
        [DataMember]
        public Aqiinfo aqiinfo { get; set; }
    }

    [DataContract]
    public class Night
    {
        [DataMember]
        public string weather { get; set; }
        [DataMember]
        public string templow { get; set; }
        [DataMember]
        public string img { get; set; }
        [DataMember]
        public string winddirect { get; set; }
        [DataMember]
        public string windpower { get; set; }
    }
    [DataContract]
    public class Day
    {
        [DataMember]
        public string weather { get; set; }
        [DataMember]
        public string temphigh { get; set; }
        [DataMember]
        public string img { get; set; }
        [DataMember]
        public string winddirect { get; set; }
        [DataMember]
        public string windpower { get; set; }
    }
    [DataContract]
    public class Daily
    {
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string week { get; set; }
        [DataMember]
        public string sunrise { get; set; }
        [DataMember]
        public string sunset { get; set; }
        [DataMember]
        public Night night { get; set; }
        [DataMember]
        public Day day { get; set; }
    }
    [DataContract]
    public class Hourly
    {
        [DataMember]
        public string time { get; set; }
        [DataMember]
        public string weather { get; set; }
        [DataMember]
        public string temp { get; set; }
        [DataMember]
        public string img { get; set; }
    }
    [DataContract]
    public class Result
    {
        [DataMember]
        public string city { get; set; }
        [DataMember]
        public string cityid { get; set; }
        [DataMember]
        public string citycode { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string week { get; set; }
        [DataMember]
        public string weather { get; set; }
        [DataMember]
        public string temp { get; set; }
        [DataMember]
        public string temphigh { get; set; }
        [DataMember]
        public string templow { get; set; }
        [DataMember]
        public string img { get; set; }
        [DataMember]
        public string humidity { get; set; }
        [DataMember]
        public string pressure { get; set; }
        [DataMember]
        public string windspeed { get; set; }
        [DataMember]
        public string winddirect { get; set; }
        [DataMember]
        public string windpower { get; set; }
        [DataMember]
        public string updatetime { get; set; }
        [DataMember]
        public List<Index> index { get; set; }
        [DataMember]
        public Aqi aqi { get; set; }
        [DataMember]
        public List<Daily> daily { get; set; }
        [DataMember]
        public List<Hourly> hourly { get; set; }
    }
    [DataContract]
    public class WeatherRootObject
    {
        [DataMember]
        public string status { get; set; }
        [DataMember]
        public string msg { get; set; }
        [DataMember]
        public Result result { get; set; }

        public WeatherRootObject()
        {

        }
    }
}
