using MyIntelligentHomeSystem.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyIntelligentHomeSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        private List<Settings> settings;
        public SettingsPage()
        {
            this.InitializeComponent();
            settings = new List<Settings>()
            {
                new Settings{Title="房间",Icon="ms-appx:///Assets/SettingsPage/rooms.png"},
                new Settings{Title="人员",Icon="ms-appx:///Assets/Notification/stranger.jpg"}
            };
            SettingsListView.ItemsSource = settings;

            ImageBrush imageBrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/BingWallPaper/DevicePage22.jpg"))
            };
            SettingsPage_grid.Background = imageBrush;
                
        }

        private void MainPgaeNavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

        }

        private void SettingsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            Settings settings = e.ClickedItem as Settings;
            switch (settings.Title)
            {
                case "房间":Frame.Navigate(typeof(SettingRoomPage));
                    break;
                case "人员":
                    break;
                default:
                    break;
            }
        }

        //private void NavigatedtoSettingRoomPage()
        //{
        //    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("image",);
        //    Frame.Navigate(typeof(DestinationPage));
        //}
    }
}
