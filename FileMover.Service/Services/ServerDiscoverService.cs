using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FileMover.Service.Services
{
    /// <summary>
    /// 服务器发现服务。监听所有IP，收到多播后，获取服务器实际使用的IP，然后返回给客户端
    /// </summary>
    public class ServerDiscoverService
    {
        /// <summary>
        /// 用来被加锁的对象，初始化或者重新初始化监听器的时候用
        /// </summary>
        private object _lockObj = new object();


        /// <summary>
        /// 多播监听器，用来接收客户端发来的多播信息
        /// </summary>
        private Socket multicastListener;
        private int _port;

        ServerDiscoverService(int port)
        {
            _port = port;
        }

        public int Port
        {
            get { return _port; }
            set
            {
                _port = value;
                Start();
            }
        }


        public void Start()
        {
            lock (_lockObj)
            {

            }


            multicastListener = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            multicastListener.Bind(new IPEndPoint(IPAddress.IPv6Any, Port));
            var multicastOption = new IPv6MulticastOption(IPAddress.Parse("FF12::1206"));
            multicastListener.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
            Logger<int> ff = new Logger<int>(null);
            //((ILogger)ff)

        }

    }
}
