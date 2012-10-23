using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TankGameNetworkAndSound.Network
{
    public enum Protocol
    {
        Disconnected = 0,
        Connected = 1,
        PlayerMoved = 2,
        BulletCreated = 3,
        PlayerRotated = 4
    }
}
