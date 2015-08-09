using Windows.Networking.Connectivity;

namespace NetworkClient
{
    public class NetworkConnection
    {
        private static bool _isConnected = false;
        private static bool _isInternetAvailable = false;
        private static bool _isRoaming = false;
        private static bool _isLowOnData = false;
        private static bool _isOverDataLimit = false;
        private static bool _isWifiConnected = false;

        public static bool IsConnected { get { return _isConnected; } }
        public static bool IsInternetAvailable { get { return _isInternetAvailable; } }
        public static bool IsRoaming { get { return _isRoaming; } }
        public static bool IsLowOnData { get { return _isLowOnData; } }
        public static bool IsOverDataLimit { get { return _isOverDataLimit; } }
        public static bool IsWifiConnected { get { return _isWifiConnected; } }


        public static void UpdateNetworkInformation()
        {
            _isConnected = false;
            _isInternetAvailable = false;
            _isRoaming = false;
            _isLowOnData = false;
            _isOverDataLimit = false;
            _isWifiConnected = false;

            // Get current Internet Connection Profile.
            ConnectionProfile internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            // Air plane mode is on...
            if (internetConnectionProfile == null)
            {
                _isConnected = false;
                return;
            }

            _isConnected = true;
            // If true, internet is accessible.
            _isInternetAvailable = internetConnectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess;

            // Check the connection details.
            if (internetConnectionProfile.NetworkAdapter.IanaInterfaceType != 71)// Connection is not a Wi-Fi connection. 
            {
                ConnectionCost connectionCost = internetConnectionProfile.GetConnectionCost();

                _isRoaming = connectionCost.Roaming;

                // User is Low on Data package only send low data.
                _isLowOnData = connectionCost.ApproachingDataLimit;

                // User is over limit do not send data
                _isOverDataLimit = connectionCost.OverDataLimit;
            }
            else //Connection is a Wi-Fi connection. Data restrictions are not necessary. 
            {
                _isWifiConnected = true;
            }
        }
    }
}
