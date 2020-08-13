using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace FileMover.Service.Utils
{
    public static class IP_Util
    {
        public static IPAddress GetLocalIPv6Address()
        {
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var i in interfaces)
            {
                if (i.OperationalStatus == OperationalStatus.Up)
                {
                    var ipv6Address = i.GetIPProperties().UnicastAddresses.Where(t => t.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6);
                    var r = ipv6Address.Where(t => t.Address.IsIPv6LinkLocal);
                }
            }
            return null;
        }
    }
}
