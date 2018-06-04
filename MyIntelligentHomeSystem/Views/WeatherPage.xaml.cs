using MyIntelligentHomeSystem.Helpers;
using MyIntelligentHomeSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyIntelligentHomeSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WeatherPage : Page
    {
        private const string APIkey = "**************";

        
        private static Geoposition geoposition;
        private static WeatherRootObject rootObject;
        private static int LastHourOfTime;
        private static short Pageloadnum=0;
        private TodayWeather todayWeather;
        private bool IsUnauthorizedAccess = true;
        public WeatherPage()
        {
            this.InitializeComponent();
            todayWeather = new TodayWeather();
            LastHourOfTime = System.DateTime.Now.Date.Hour;
            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/HomePaper/Win101.jpg"))
            };
            WeatherPageGrid.Background = imageBrush;

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            WeatherpagePrograssRing.IsActive = true;
            WeatherpagePrograssRing.Visibility = Visibility.Visible;
            TempHighFontIcon.Visibility = Visibility.Collapsed;
            TempLowFontIcon.Visibility = Visibility.Collapsed;
            AqiTblock.Visibility = Visibility.Collapsed;

            if (MyNetHelper.IsNetAdapterWork()!=true)
            {
                    var dialog = new MessageDialog("请检查网络适配器是否打开且正常工作", "网络适配器错误");
                await dialog.ShowAsync();
            }
            else if (MyNetHelper.IsNetConnected()!=true)
            {
                var dialog = new MessageDialog("请联系当地网络运营服务商", "网络连接错误");
                await dialog.ShowAsync();
            }
            else
            {
                if (geoposition==null)
                {
                    try
                    {
                        geoposition = await MyGeopositionHelper.GetGeopositionasync();
                        if (geoposition!=null)
                        {
                            IsUnauthorizedAccess = false;//
                        }
                    }
                    catch (System.UnauthorizedAccessException)
                    {
                        MyMessageDialogHelper.ShowUnauthorizedAccessMessageAsync();                        
                    }
                    if(IsUnauthorizedAccess==false&&Pageloadnum != 0)  //如果不是第一次访问
                    {
                        if (LastHourOfTime - System.DateTime.Now.Date.Hour > 2) //时间距离上次访问是否大于2小时
                        {
                            rootObject = await MyWeatherHelper.GetWeatherRootObjectAsync(APIkey, geoposition.Coordinate.Point.Position.Latitude, geoposition.Coordinate.Point.Position.Longitude);
                            Pageloadnum++;
                            LastHourOfTime = System.DateTime.Now.Date.Hour;
                        }
                    }
                    else
                    {
                        rootObject = await MyWeatherHelper.GetWeatherRootObjectAsync(APIkey, 39.088, 117.696184);
                        Pageloadnum++;
                    }

                }

                if (rootObject.msg!="ok")
                {
                    MyMessageDialogHelper.ShowApiErrorMessageAsync(rootObject.status, rootObject.msg);
                }
                else
                {
                    todayWeather.CityName = rootObject.result.city;  //天津
                    todayWeather.Temp = rootObject.result.temp + "°";         //23
                    todayWeather.TempHigh = rootObject.result.temphigh + "°";     //30
                    todayWeather.TempLow = rootObject.result.templow + "°";   //10
                    todayWeather.WeatherDescription = rootObject.result.weather; //小雨
                    todayWeather.LastUpdateTime = rootObject.result.updatetime;     //2018-04-12    
                    todayWeather.AirQualityIndex = rootObject.result.aqi.aqi;       //83
                    todayWeather.AqiLevel = "级别:"+rootObject.result.aqi.quality;      //良 
                    //todayWeather.Icon = $"ms-appx:///Assets/weathercn02/{rootObject.result.img}.png";              //0.png
                    todayWeather.Icon = string.Format($"ms-appx:///Assets/weathercn02/{rootObject.result.img}.png");
                    todayWeather.AqiColor = rootObject.result.aqi.aqiinfo.color;    //#FF7E00
                    todayWeather.Humidity = "湿度:"+rootObject.result.humidity+"％";         //83
                    DaliyWeatherListView.ItemsSource = DaliyWeather.GetWeathers(rootObject);
                }
                WeatherpagePrograssRing.IsActive = false;
                WeatherpagePrograssRing.Visibility = Visibility.Collapsed;
                TempHighFontIcon.Visibility = Visibility.Visible;
                TempLowFontIcon.Visibility = Visibility.Visible;
                AqiTblock.Visibility = Visibility.Visible;
            }
        }
    }
}
