using FileMover.Service.Utils;
using Google.Protobuf;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace FileMover.Service.Services
{
    /// <summary>
    /// UDP服务
    /// </summary>
    public class UDPService : IHostedService
    {
        /// <summary>
        /// 接受多播用的端口
        /// </summary>
        const int MULTICAST_PORT = 20086;

        /// <summary>
        /// 传输用端口
        /// </summary>
        const int TRANSMISSION_PORT = 20087;

        Socket multicastSocket;
        Socket transmissionSocket;

        private UdpClient udpListener;
        private byte[] buffer;
        private MemoryStream sendBuffer;
        private Task listenTask;

        public UDPService()
        {
            IP_Util.GetLocalIPv6Address();


            transmissionSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            transmissionSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, 20086));
            var multicastOption = new IPv6MulticastOption(IPAddress.Parse("FF12::1206"));
            transmissionSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);


            udpListener = new UdpClient(new IPEndPoint(IPAddress.IPv6Any, 20086));
            udpListener.JoinMulticastGroup(IPAddress.Parse("FF12::1206"));
            buffer = new byte[1024];
            sendBuffer = new MemoryStream(buffer);
        }

        public async void Listen()
        {


            var a = udpListener.ReceiveAsync();

            var receiveInfo = await udpListener.ReceiveAsync();
            var buffer = receiveInfo.Buffer;

            if (buffer[0] == 0x00 && buffer[1] == 0x01 && buffer[2] == 0x02)
            {
                var serverInformation = new ServerInformation() { FreeSpace = 2333, IPAddress = receiveInfo.RemoteEndPoint.Address.ToString(), Port = receiveInfo.RemoteEndPoint.Port };
                sendBuffer.SetLength(0);
                serverInformation.WriteTo(sendBuffer);
                udpListener.Send(buffer, (int)sendBuffer.Length);
            }

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            multicastSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            multicastSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, MULTICAST_PORT));
            var multicastOption = new IPv6MulticastOption(IPAddress.Parse("FF12::1206"));
            multicastSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
