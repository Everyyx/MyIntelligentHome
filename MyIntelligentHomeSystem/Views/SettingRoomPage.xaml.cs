using MyIntelligentHomeSystem.Data;
using MyIntelligentHomeSystem.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class SettingRoomPage : Page
    {

        public class ImageListClass
        {
            public string ImagePath { get; set; }
        }
        public SettingRoomPage()
        {
            this.InitializeComponent();

            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BingWallPaper/settingPagesave.jpg"))
            };
            SettingRoomPage_grid.Background = imageBrush;


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

            UpdateListBox();
            UpdateImages();

            if (MainPage.MyHome.Rooms.Count > 0)
            {
                UpdateRoomDetailPane(MainPage.MyHome.Rooms[0]);
                if (MainPage.MyHome.Rooms[0].Devices.Count > 0)
                {
                    UpdateDeviceDetailPane(MainPage.MyHome.Rooms[0], true);
                }
            }
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

        private void Btn_AddRoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Room NewRoom = new Room();

            NewRoom.I2C_Slave_Address = 0x40;
            NewRoom.ImagePath = "ms-appx:///Resources/Image/Tiles/Room/Flower1.png";
            NewRoom.Name = "My Room";

            MainPage.MyHome.Rooms.Add(NewRoom);

            UpdateListBox(1);
        }

        private void Btn_RemoveRoom_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Rooms.Items.Count > 0 && LB_Rooms.SelectedIndex != -1)
            {
                MainPage.MyHome.Rooms.Remove((Room)LB_Rooms.SelectedItem);
                LB_Rooms.Items.Remove(LB_Rooms.SelectedItem);
            }
            Txt_RoomName.Text = "";
            Txt_RoomI2CAddress.Text = "";
            UpdateListBox();
        }

        private void LB_Rooms_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Rooms.Items.Count > 0)
            {
                UpdateRoomDetailPane((Room)LB_Rooms.SelectedItem);
                UpdateDeviceDetailPane((Room)LB_Rooms.SelectedItem, true);
            }
        }

        private void Btn_Save_Tapped(object sender, TappedRoutedEventArgs e)
        {            

        }

        private void Btn_AddDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Device NewDevice = new Device
            {
                Name = "MyDevice",
                ImagePath = "ms-appx:///Assets/Device/Plug_130.png",
                Pin = Device.PinsEnum.D0
            };
            if (LB_Rooms.SelectedIndex>-1)
            {
                MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].AddDevice(NewDevice);
            }
            else
            {
                MyMessageDialogHelper.ShowNormalErrorMessageAsync("请选择一个房间！");
            }
            // Only update device listboc
            UpdateListBox(2);
        }

        private void Btn_RemoveDevice_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Devices.Items.Count > 0 && LB_Devices.SelectedIndex != -1)
            {
                MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].Devices.Remove((Device)LB_Devices.SelectedItem);
                LB_Devices.Items.Remove(LB_Devices.SelectedItem);
            }
            UpdateListBox();
        }

        private void LB_Devices_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (LB_Devices.Items.Count > 0)
            {
                UpdateDeviceDetailPane((Room)LB_Rooms.SelectedItem, false, true);
            }
        }

        private void UpdateListBox(byte Mode = 0)
        {
            if (Mode == 0 || Mode == 1)
            {
                LB_Rooms.Items.Clear();
            }

            if (MainPage.MyHome.Rooms.Count > 0) //
            {
                if (Mode == 0 || Mode == 1)
                {
                    foreach (var Room in MainPage.MyHome.Rooms)
                    {
                        LB_Rooms.Items.Add(Room);
                    }

                    LB_Rooms.SelectedIndex = 0;
                }

                if (Mode == 0 || Mode == 2)
                {
                    LB_Devices.Items.Clear();
                    if (MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].Devices.Count > 0)
                    {
                        foreach (var Device in MainPage.MyHome.Rooms[LB_Rooms.SelectedIndex].Devices)
                        {
                            LB_Devices.Items.Add(Device);
                            Debug.WriteLine(Device.Id);
                            Debug.WriteLine(Device.Name);
                            Debug.WriteLine(Device.Pin.ToString());
                            Debug.WriteLine(Device.ImagePath);
                        }
                        LB_Devices.SelectedIndex = 0;
                    }
                }
            }
        }

        private void UpdateRoomDetailPane(Room SelectedRoom)
        {
            Txt_RoomName.Text = SelectedRoom.Name;
            Txt_RoomI2CAddress.Text = "0x" + SelectedRoom.I2C_Slave_Address.ToString("X");

            foreach (var _Image in LV_RoomImage.Items)
            {
                if (((ImageListClass)_Image).ImagePath == SelectedRoom.ImagePath)
                {
                    LV_RoomImage.SelectedItem = _Image;
                    break;
                }
            }
        }

        private void UpdateImages()
        {
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Attic.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Basement.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Fireplace.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Flower1.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Flower2.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Flower3.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Kitchen.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Pillow.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Room.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/TV.png" });
            LV_RoomImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Room/Window.png" });

            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Device/Bulb_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Device/Fan_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Device/Lamp1_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Device/Lamp2_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Device/Plug_130.png" });
            LV_DeviceImage.Items.Add(new ImageListClass { ImagePath = "ms-appx:///Assets/Device/Socket_130.png" });
        }


        private void UpdateDeviceDetailPane(Room SelectedRoom, bool SelectFirst = false, bool SkipClear = false)
        {
            if (SkipClear == false)
            {
                ClearDeviceDetailPane();
            }
            if (SelectedRoom.Devices.Count > 0)
            {
                if (SkipClear == false)
                {
                    foreach (var _Device in SelectedRoom.Devices)
                    {
                        LB_Devices.Items.Add(_Device);
                    }
                }

                if (SelectFirst)
                {
                    LB_Devices.SelectedIndex = 0;
                }

                Device SelectedDevice = (Device)LB_Devices.SelectedItem;

                txt_DeviceName.Text = SelectedDevice.Name;

                foreach (ComboBoxItem item in cmb_DevicePin.Items)
                {
                    if ((string)item.Content == SelectedDevice.Pin.ToString())
                    {
                        cmb_DevicePin.SelectedItem = item;
                        break;
                    }
                }

                foreach (var _Image in LV_DeviceImage.Items)
                {
                    if (((ImageListClass)_Image).ImagePath == SelectedDevice.ImagePath)
                    {
                        LV_DeviceImage.SelectedItem = _Image;
                        break;
                    }
                }
            }
        }
        private void ClearDeviceDetailPane()
        {
            if (LB_Devices.Items != null && LB_Devices.Items.Count > 0)
            {
                LB_Devices.Items.Clear();
            }
            txt_DeviceName.Text = "";
            cmb_DevicePin.SelectedIndex = -1;
            LV_DeviceImage.SelectedIndex = -1;
        }

        private void Bt_Save_Click(object sender, RoutedEventArgs e)
        {
            Bt_Save.IsEnabled = false;
            if (LB_Rooms.Items.Count > 0 && LB_Rooms.SelectedIndex != -1)
            {
                // Update Room Detail
                Room SelectedRoom = (Room)LB_Rooms.SelectedItem;
                SelectedRoom.Name = Txt_RoomName.Text;
                int NewAddress = 0;
                IFormatProvider _Culture = new CultureInfo("en-US");
                int.TryParse(Txt_RoomI2CAddress.Text.Replace("0x", ""), System.Globalization.NumberStyles.HexNumber, _Culture, out NewAddress);
                SelectedRoom.I2C_Slave_Address = NewAddress;
                SelectedRoom.ImagePath = ((ImageListClass)LV_RoomImage.SelectedItem).ImagePath;
                // Update Device Detail
                if (LB_Devices.Items.Count > 0)
                {
                    if (LB_Devices.SelectedIndex != -1)
                    {
                        Device SelectedDevice = (Device)LB_Devices.SelectedItem;
                        SelectedDevice.Name = txt_DeviceName.Text;
                        SelectedDevice.Pin = (Device.PinsEnum)Enum.Parse(typeof(Device.PinsEnum), ((ComboBoxItem)cmb_DevicePin.SelectedItem).Content.ToString());
                        SelectedDevice.ImagePath = ((ImageListClass)LV_DeviceImage.SelectedItem).ImagePath;
                    }
                }
            }
            // Save
            Home.SaveHome(MainPage.MyHome);
            Tb_saveStatues.Text = "Ok！已保存！";
            Bt_Save.IsEnabled = true;
        }
    }
}
