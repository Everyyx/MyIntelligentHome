using MyIntelligentHomeSystem.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
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
    public sealed partial class DevicePage : Page
    {
        public DevicePage()
        {
            this.InitializeComponent();

            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BingWallPaper/Devicepage.jpg"))
            };
            DevicePageGrid.Background = imageBrush;

            KeyboardAccelerator GoBack = new KeyboardAccelerator();
            GoBack.Key = VirtualKey.GoBack;
            GoBack.Invoked += BackInvoked;
            KeyboardAccelerator AltLeft = new KeyboardAccelerator();
            AltLeft.Key = VirtualKey.Left;
            AltLeft.Invoked += BackInvoked;
            this.KeyboardAccelerators.Add(GoBack);
            this.KeyboardAccelerators.Add(AltLeft);
            // ALT routes here
            AltLeft.Modifiers = VirtualKeyModifiers.Menu;
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }
        private class DeviceInfo : Device
        {
            public string Tooltip { get; set; }
            public SolidColorBrush StatusColor { get; set; }
            public int SlaveAddress; // <-- need to use to avoid unknown error in-place of I2C_Slave_Address.
        }
        Room selectedroom;

        public bool SensorCollectorAlreadyWorking = false;
        private CancellationTokenSource _CTS = new CancellationTokenSource();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var RawData = e.Parameter as object[];
            selectedroom = (Room)RawData[0];

            UpdateTexts();
            LoadDevices();

            UpdateSensorData();
        }

        private void UpdateSensorData()
        {
            if (SensorCollectorAlreadyWorking)
            {
                return;
            }

            SensorCollectorAlreadyWorking = true;

            RoomPage.SensorData.I2C_Slave_Address = selectedroom.I2C_Slave_Address;

            Task Task_CollectSensorData = new Task(async () =>
            {
                try
                {
                    while (_CTS.IsCancellationRequested == false)
                    {

                        await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                         () =>
                         {
                             LightIntensity.Text = RoomPage.SensorData.Sensors.AmbientLight.RawData.ToString();//

                             PIR_Status.Text = (RoomPage.SensorData.Sensors.PassiveIR.HumanDetected == true) ? "Detected" : "None";
                             if (RoomPage.SensorData.Sensors.PassiveIR.HumanDetected)
                             {
                                 Img_PIR_Status.Source = new BitmapImage(new Uri("ms-appx:///Resources/Image/Common/HumanDetected_48.png"));
                             }
                             else
                             {
                                 Img_PIR_Status.Source = new BitmapImage(new Uri("ms-appx:///Resources/Image/Common/HumanDetected_None_48.png"));
                             }


                             Temp_C.Text = RoomPage.SensorData.Sensors.Temperature.Celsius.ToString() + " °C";
                         });

                        //_CTS.Token.ThrowIfCancellationRequested();
                        await Task.Delay(1000);
                    }
                }
                catch (Exception ex)
                {

                }
            }, _CTS.Token);

            Task_CollectSensorData.Start();
        }

        private void UpdateTexts()
        {
            RoomName.Text = selectedroom.Name;
        }

        private void LoadDevices()
        {
            if (LV_Devices.Items != null && (LV_Devices.Items.Count > 0))
            {
                LV_Devices.Items.Clear();
            }
            foreach (var Device in selectedroom.Devices)
            {
                DeviceInfo _DevInfo = new DeviceInfo();
                _DevInfo.Id = Device.Id;
                _DevInfo.ImagePath = Device.ImagePath;
                _DevInfo.Name = Device.Name;
                _DevInfo.Pin = Device.Pin;
                _DevInfo.SlaveAddress = Device.I2C_Slave_Address;
                //._DevInfo.IsOn = (Device.Status==Library.Core.Device.StatusEnum.On)?true:false;

                // Invert Logic due to delay in task completion (TurnOn and TurnOff functions)
                if (Device.Status == Device.StatusEnum.On)
                {

                    _DevInfo.StatusColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 255, 50));
                }
                else
                {
                    _DevInfo.StatusColor = new SolidColorBrush(Windows.UI.Color.FromArgb(150, 128, 128, 128));
                }
                LV_Devices.Items.Add(_DevInfo);
            }
        }


        private void LV_Devices_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DeviceInfo _SelDevInfo = (DeviceInfo)LV_Devices.SelectedItem;
            Device SelectedDevice = new Device();

            foreach (var Device in selectedroom.Devices)
            {
                if (_SelDevInfo.Id == Device.Id)
                {
                    SelectedDevice = Device;
                    break;
                }
            }


            if (SelectedDevice.Status == Device.StatusEnum.Off)
            {
                Task.Factory.StartNew(() =>
                {
                    SelectedDevice.TurnOn();
                }).Wait(1000);
                SelectedDevice.Status = Device.StatusEnum.On;
            }
            else
            {
                Task.Factory.StartNew(() =>
                {
                    SelectedDevice.TurnOff();
                }).Wait(1000);
                SelectedDevice.Status = Device.StatusEnum.Off;
            }

            LoadDevices();
        }
    }
}
