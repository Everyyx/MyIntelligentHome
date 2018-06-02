using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using MyIntelligentHomeSystem.ViewModels;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyIntelligentHomeSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public HomePage()
        {
            this.InitializeComponent();
            ImageBrush imagebrush = new ImageBrush()
            {
                ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/HomePaper/Win10.jpg"))//https://bing.ioliu.cn/v1/rand
            };
            HomePageGrid.Background = imagebrush;

            Task.Factory.StartNew(async () =>
            {
                while(true)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                     {
                         this.DataContext = new TimeViewModel();
                     });
                    await Task.Delay(60000);
                }
            }
            );

            Task.Factory.StartNew(async () =>
            {
                while (true)
                {
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                    {
                        ImageBrush Taskimagebrush= new ImageBrush()
                        {
                            ImageSource = new BitmapImage(new Uri("https://bing.ioliu.cn/v1/rand"))//https://bing.ioliu.cn/v1/rand
                        };
                        HomePageGrid.Background = Taskimagebrush;
                    });
                    await Task.Delay(3600000);   //3600000一小时更新一次
                }
            }
            );
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
   
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {            
            base.OnNavigatedTo(e);
        }
    }
}
