using System;
using System.Collections;
using System.Collections.Generic;

namespace Microsoft.Win32.WindowsShell.NetworkConnections
{
    internal sealed class NetworkCollection : IEnumerable<Network>
    {
        private IEnumerable nativeEnumerable;
        internal NetworkCollection(IEnumerable native)
        {
            nativeEnumerable = native;
        }

        public IEnumerator<Network> GetEnumerator()
        {
            foreach (INetwork iface in nativeEnumerable)
            {
                yield return new Network(iface);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (INetwork iface in nativeEnumerable)
            {
                yield return new Network(iface);
            }
        }
    }

    internal sealed class NetworkConnectionCollection : IEnumerable<NetworkConnection>
    {
        private IEnumerable nativeEnumerable;
        internal NetworkConnectionCollection(IEnumerable native)
        {
            nativeEnumerable = native;
        }

        public IEnumerator<NetworkConnection> GetEnumerator()
        {
            foreach (INetworkConnection iface in nativeEnumerable)
            {
                yield return new NetworkConnection(iface);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (INetworkConnection iface in nativeEnumerable)
            {
                yield return new NetworkConnection(iface);
            }
        }
    }
}
