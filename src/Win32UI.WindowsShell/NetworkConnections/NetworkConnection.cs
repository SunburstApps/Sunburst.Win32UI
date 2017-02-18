using System;

namespace Microsoft.Win32.WindowsShell.NetworkConnections
{
    public sealed class NetworkConnection
    {
        private INetworkConnection connection;
        internal NetworkConnection(INetworkConnection iface)
        {
            connection = iface;
        }

        public Network Network => new Network(connection.GetNetwork());
        public Guid AdapterId => connection.GetAdapterId();
        public Guid ConnectionId => connection.GetConnectionId();
        public ConnectivityState Connectivity => connection.GetConnectivity();
        public NetworkDomainType DomainType => connection.GetDomainType();
        public bool IsConnectedToInternet => connection.IsConnectedToInternet;
        public bool IsConnected => connection.IsConnected;
    }
}
