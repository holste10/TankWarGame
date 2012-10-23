using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TankGameNetworkAndSound.Network
{
    public interface INetworkService
    {
        MemoryStream GetReadStream();
        MemoryStream GetWriteStream();
        BinaryReader GetReader();
        BinaryWriter GetWriter();
        void SendData(byte[] data);
        byte[] GetDataFromMemoryStream(MemoryStream ms);
    }
}
