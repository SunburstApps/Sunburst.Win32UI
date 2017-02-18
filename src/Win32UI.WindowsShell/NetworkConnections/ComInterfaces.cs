using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace Microsoft.Win32.WindowsShell.NetworkConnections
{
    [ComImportAttribute, GuidAttribute("DCB00002-570F-4A9B-8D69-199FDBA5723B")]
    internal interface INetwork
    {
        [return: MarshalAsAttribute(UnmanagedType.BStr)]
        string GetName();
        void SetName([In, MarshalAsAttribute(UnmanagedType.BStr)] string name);
        [return: MarshalAsAttribute(UnmanagedType.BStr)]
        string GetDescription();
        void SetDescription([In, MarshalAsAttribute(UnmanagedType.BStr)] string description);
        Guid GetNetworkId();
        NetworkDomainType GetDomainType();
        [return: MarshalAsAttribute(UnmanagedType.Interface)]
        IEnumerable GetNetworkConnections();
        void GetTimeCreatedAndConnected(out uint createdTimeLow, out uint createdTimeHigh, out uint connectedTimeLow, out uint connectedTimeHigh);
        bool IsConnectedToInternet { get; }
        bool IsConnected { get; }
        ConnectivityState GetConnectivity();
        NetworkTrustLevel GetCategory();
        void SetCategory(NetworkTrustLevel level);
    }

    [ComImportAttribute, GuidAttribute("DCB00005-570F-4A9B-8D69-199FDBA5723B")]
    internal interface INetworkConnection
    {
        [return: MarshalAsAttribute(UnmanagedType.Interface)]
        INetwork GetNetwork();
        bool IsConnectedToInternet { get; }
        bool IsConnected { get; }
        ConnectivityState GetConnectivity();
        Guid GetConnectionId();
        Guid GetAdapterId();
        NetworkDomainType GetDomainType();
    }

    [ComImportAttribute, GuidAttribute("DCB00000-570F-4A9B-8D69-199FDBA5723B")]
    internal interface INetworkListManager
    {
        [return: MarshalAsAttribute(UnmanagedType.Interface)]
        IEnumerable GetNetworks(GetNetworksFlags flags);
        [return: MarshalAsAttribute(UnmanagedType.Interface)]
        INetwork GetNetwork([In] Guid networkId);
        [return: MarshalAsAttribute(UnmanagedType.Interface)]
        IEnumerable GetNetworkConnections();
        [return: MarshalAsAttribute(UnmanagedType.Interface)]
        INetworkConnection GetNetworkConnection(Guid connectionId);
        bool IsConnectedToInternet { get; }
        bool IsConnected { get; }
        ConnectivityState GetConnectivity();
    }

    [ComImportAttribute, GuidAttribute("DCB00C01-570F-4A9B-8D69-199FDBA5723B")]
    [ClassInterfaceAttribute(ClassInterfaceType.None)]
    internal class CNetworkListManager { }
}
