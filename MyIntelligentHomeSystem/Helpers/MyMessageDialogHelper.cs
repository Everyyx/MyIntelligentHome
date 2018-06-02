using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace MyIntelligentHomeSystem.Helpers
{
    public class MyMessageDialogHelper
    {

        public static async void ShowApiErrorMessageAsync(string status, string msg)
        {
            var dialog = new MessageDialog($"错误代码：{status}，提示信息：{msg}", "API访问错误");
            await dialog.ShowAsync();
        }

        public static async void ShowWebcamErrorMessageAsync(string normal = "该应用被拒绝访问摄像头，如需使用该功能，请在设置中允许相应权限")
        {
            var dialog = new MessageDialog(normal, "摄像头访问错误");
            await dialog.ShowAsync();
        }

        public static async void ShowUnauthorizedAccessMessageAsync(string normal = "该应用被拒绝访问详细地址，如需使用该功能，请在设置中允许相应权限")
        {
            var dialog = new MessageDialog(normal, "访问地址错误");
            await dialog.ShowAsync();
        }

        public static async void ShowNormalErrorMessageAsync(string normal = "")
        {
            var dialog = new MessageDialog(normal, "错误");
            await dialog.ShowAsync();
        }
    }
}
