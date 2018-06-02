using Microsoft.Azure.NotificationHubs;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyIntelligentHomeSystem.Helpers
{
    public class MyNotificationHelper
    {
        public async static void SendNotification(string imagepath, string visitoename = "未知", string visitordata = "陌生人")
        {
            NotificationHubClient hub = NotificationHubClient
                .CreateClientFromConnectionString(
                    "Endpoint=sb://myazurenotificationhub.servicebus.chinacloudapi.cn/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=RCMPTgccfIQagUo+xEjLGQLldY6X55oRlb7qyXdVcqg="
                    , "MyIntelligentHome");
            if (visitordata != "陌生人")
            {
                var toastContent = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "嗨！o(*￣▽￣*)ブ，你有新的访客来了~~~"
                            },
                            new AdaptiveText()
                            {
                                Text = $"好像是{visitordata},{visitoename}"
                            },
                            new AdaptiveImage()
                            {
                                Source = imagepath
                            }
                        },
                            AppLogoOverride = new ToastGenericAppLogo()
                            {
                                Source = "https://unsplash.it/64?image=669",
                                HintCrop = ToastGenericAppLogoCrop.Circle
                            }
                        }
                    }
                };

                string toast = toastContent.GetContent();
                await hub.SendWindowsNativeNotificationAsync(toast);
            }
            else
            {
                var toastContent = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                        {
                            new AdaptiveText()
                            {
                                Text = "(⊙﹏⊙)！有陌生人找你！！············"
                            },
                            new AdaptiveText()
                            {
                                Text = $"好像是{visitordata},{visitoename}"
                            },
                            new AdaptiveImage()
                            {
                                Source = imagepath
                            }
                        },
                            AppLogoOverride = new ToastGenericAppLogo()
                            {
                                Source = "https://unsplash.it/64?image=669",
                                HintCrop = ToastGenericAppLogoCrop.Circle
                            }
                        }
                    }
                };

                string toast = toastContent.GetContent();
                await hub.SendWindowsNativeNotificationAsync(toast);
            }
        }
    }
}
