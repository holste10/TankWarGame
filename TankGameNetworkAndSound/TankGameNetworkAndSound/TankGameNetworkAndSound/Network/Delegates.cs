using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using TankGameNetworkAndSound.GameObjects;

namespace TankGameNetworkAndSound.Network
{
    public delegate void ConnectionEvent(object sender, TcpClient user);
    public delegate void DataReceivedEvent(TcpClient sender, byte[] data);
    public delegate Tank PlayerConnectedEvent();
}
