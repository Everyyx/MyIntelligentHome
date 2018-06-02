using MyIntelligentHomeSystem.Data;
using MyIntelligentHomeSystem.Helpers;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyIntelligentHomeSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RoomPage : Page
    {


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


        public RoomPage()
        {
            this.InitializeComponent();

            UpdateListBox();

            if (MainPage.MyHome != null && MainPage.MyHome.Rooms != null && MainPage.MyHome.Rooms.Count > 0)
            {
                SensorData.I2C_Slave_Address = MainPage.MyHome.Rooms[0].I2C_Slave_Address;
                SensorData.CollectData();
            }

            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BingWallPaper/Roompage.jpg"))
            };
            RoomPageGrid.Background = imageBrush;
        }

        private void UpdateListBox()
        {
            if (MainPage.MyHome.Rooms.Count > 0)
            {
                foreach (var Room in MainPage.MyHome.Rooms)
                {
                    Rooms.Items.Add(Room);
                }
            }
        }

        private void Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(DevicePage), new object[] { Rooms.SelectedItem });
        }
    }
}
