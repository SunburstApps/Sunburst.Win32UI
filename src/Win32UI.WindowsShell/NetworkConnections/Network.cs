using System;
using System.Collections.Generic;

namespace Microsoft.Win32.WindowsShell.NetworkConnections
{
    public sealed class Network
    {
        // Resume at: Network.CreatedTime (Network.cs line 99)

        private INetwork network;
        internal Network(INetwork iface)
        {
            network = iface;
        }

        public NetworkTrustLevel TrustLevel
        {
            get => network.GetCategory();
            set => network.SetCategory(value);
        }

        public DateTime ConnectedTime
        {
            get
            {
                network.GetTimeCreatedAndConnected(out uint unused1, out uint unused2, out uint low, out uint high);
                long time = high;
                time <<= 32;
                time |= low;
                return DateTime.FromFileTimeUtc(time);
            }
        }

        public IEnumerable<NetworkConnection> Connections => new NetworkConnectionCollection(network.GetNetworkConnections());
        public ConnectivityState Connectiity => network.GetConnectivity();
    }
}
