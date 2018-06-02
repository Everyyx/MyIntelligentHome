using MyIntelligentHomeSystem.Data;
using MyIntelligentHomeSystem.Helpers;
using MyIntelligentHomeSystem.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyIntelligentHomeSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static Home MyHome;

        public static class SensorData
        {
            private static bool AlreadyRunning = false;
            public static int I2C_Slave_Address { get; set; }

            public struct SensorSturct
            {
                public AmbientLight AmbientLight;
                public PassiveIR PassiveIR;
                public Temperature Temperature;
            }

            public static SensorSturct Sensors;

            /// <summary>
            /// Collects sensor data at interval of 1 second. To get data of specific room, do not call this function instead update value to property I2C_Slave_Address and this function will collect data from that slave.
            /// </summary>
            public static void CollectData()
            {
                if (AlreadyRunning == false)
                {
                    AlreadyRunning = true;

                    Sensors = new SensorSturct();
                    Sensors.AmbientLight = new AmbientLight();
                    Sensors.PassiveIR = new PassiveIR();
                    Sensors.Temperature = new Temperature();

                    Task Task_CollectSensorData = new Task(async () =>
                    {
                        while (true)
                        {
                            var Response = await I2CHelper.WriteRead(I2C_Slave_Address, I2CHelper.Mode.Mode0);

                            // Update Ambient Light
                            Sensors.AmbientLight.RawData = (short)Response[0];

                            //Update PIR State
                            Sensors.PassiveIR.HumanDetected = (((byte)Response[1]) == 1) ? true : false;

                            // Update Temperature
                            Sensors.Temperature.Celsius = (byte)Response[3];
                            Sensors.Temperature.Celsius *= (((byte)Response[2]) == 0) ? -1 : 1; // Update Temperature Sign. Refer mode 0 for details.

                            Debug.WriteLine(Sensors.AmbientLight.RawData);
                            Debug.WriteLine(Sensors.PassiveIR.HumanDetected.ToString());
                            Debug.WriteLine(Sensors.Temperature.Celsius);
                            Debug.WriteLine("=======================");

                            await Task.Delay(1000);
                        }
                    });

                    Task_CollectSensorData.Start();
                    Task_CollectSensorData.Wait();
                }
            }
        }

        public MainPage()
        {
            this.InitializeComponent();
            MainPageRootFrame.Navigate(typeof(HomePage));
            MyHome = Home.LoadHome().Result;
        }
        
        private void MainPgaeNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainPageRootFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                switch (args.InvokedItem as string)
                {
                    case "主页":MainPageRootFrame.Navigate(typeof(HomePage));
                        break;
                    case "天气":MainPageRootFrame.Navigate(typeof(WeatherPage));
                        break;
                    case "房间":MainPageRootFrame.Navigate(typeof(RoomPage));
                        break;
                    case "来访":MainPageRootFrame.Navigate(typeof(FaceDetectPage));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
