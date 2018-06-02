using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace MyIntelligentHomeSystem.Helpers
{
    public class MyNetHelper
    {
        public  static bool _isconnected;
        public static bool IsNetAdapterWork()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }
        public static bool IsNetConnected()
        {

            ConnectionProfile profile = NetworkInformation.GetInternetConnectionProfile();
            if (profile==null)
            {
                _isconnected = false;
            }
            else
            {
                NetworkConnectivityLevel networkConnectivityLevel = profile.GetNetworkConnectivityLevel();
                _isconnected = (networkConnectivityLevel != NetworkConnectivityLevel.None);
            }
            return _isconnected;         
        }
    }
}


/*
 *Pin暂时不支持uwp
 * /
//Ping ping = new Ping();
//try
//{
//    PingReply reply = ping.Send("www.baidu.com");
//    if (reply.Status == IPStatus.Success)
//    {
//        _isconnected = true;
//    }
//}
//catch (System.Net.Sockets.SocketException ex)
//{
//    _isconnected = false;
//}
//catch (System.Exception ex)
//{
//    _isconnected = false;
//}
_isconnected = true;
*/
