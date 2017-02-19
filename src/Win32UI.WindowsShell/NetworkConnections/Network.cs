using System;
using System.Collections.Generic;

namespace Microsoft.Win32.WindowsShell.NetworkConnections
{
    public sealed class Network
    {
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

        public string Name
        {
            get => network.GetName();
            set => network.SetName(value);
        }

        public string Description
        {
            get => network.GetDescription();
            set => network.SetDescription(value);
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

        public DateTime CreatedTime
        {
            get
            {
                network.GetTimeCreatedAndConnected(out uint low, out uint high, out uint unused1, out uint unused2);
                long time = high;
                time <<= 32;
                time |= low;
                return DateTime.FromFileTimeUtc(time);
            }
        }

        public IEnumerable<NetworkConnection> Connections => new NetworkConnectionCollection(network.GetNetworkConnections());
        public ConnectivityState Connectiity => network.GetConnectivity();
        public NetworkDomainType DomainType => network.GetDomainType();
        public bool IsConnected => network.IsConnected;
        public bool IsConnectedToInternet => network.IsConnectedToInternet;
        public Guid NetworkId => network.GetNetworkId();
    }
}
