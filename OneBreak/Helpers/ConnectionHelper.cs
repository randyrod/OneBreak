using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace OneBreak.Helpers
{
    public static class ConnectionHelper
    {
        public static bool IsConnected
        {
            get
            {
                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    return false;
                }

                var profile = NetworkInformation.GetInternetConnectionProfile();

                var level = profile.GetNetworkConnectivityLevel();

                switch (level)
                {
                    case NetworkConnectivityLevel.None:
                    case NetworkConnectivityLevel.LocalAccess:
                        return false;
                    case NetworkConnectivityLevel.ConstrainedInternetAccess:
                    case NetworkConnectivityLevel.InternetAccess:
                    default:
                        return true;
                }
            }
        }
    }
}
