using System;

namespace Microsoft.Win32.WindowsShell.NetworkConnections
{
    [FlagsAttribute]
    public enum ConnectivityState
    {
        NoConnectivity = 0,
        IPv4Internet = 0x40,
        IPv4LocalNetwork = 0x20,
        IPv4NoTraffic = 1,
        IPv4Subnet = 0x10,
        IPv6Internet = 0x400,
        IPv6LocalNetwork = 0x200,
        IPv6NoTraffic = 0x2,
        IPv6Subnet = 0x100
    }

    public enum NetworkDomainType
    {
        NonDomain = 0,
        Domain = 1,
        DomainAuthenticated = 2
    }

    public enum NetworkTrustLevel
    {
        Public,
        Private,
        DomainAuthenticated
    }

    [FlagsAttribute]
    public enum GetNetworksFlags
    {
        ConnectedOnly = 1,
        DisconnectedOnly = 2,
        AllNetworks = 3
    }
}
