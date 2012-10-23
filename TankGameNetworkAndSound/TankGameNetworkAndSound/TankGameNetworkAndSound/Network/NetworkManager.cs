using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.IO;
using Microsoft.Xna.Framework;
using System.Windows.Forms;
using TankGameNetworkAndSound.GameObjects;

namespace TankGameNetworkAndSound.Network
{
    public class NetworkManager : GameComponent, INetworkService
    {
        private Game _game;

        private TcpClient _client;
        private const String IP = "127.0.0.1"; // Should come from a config file
        private const int PORT = 1490;
        private const int BUFFER_SIZE = 2048;
        private byte[] _readBuffer;

        private MemoryStream _readStream;
        private MemoryStream _writeStream;
        private BinaryReader _reader;
        private BinaryWriter _writer;

        public bool enemyConnected;
        public bool enemyCreated = false;

        public TankDataHolder tankDataHolder;

        public NetworkManager(Game game)
            : base(game)
        {
            _game = game;
            _game.Components.Add(this);
            _game.Services.AddService(typeof(INetworkService), this);
        }

        public override void Initialize()
        {
            enemyConnected = false;
            _readStream = new MemoryStream();
            _writeStream = new MemoryStream();
            _reader = new BinaryReader(_readStream);
            _writer = new BinaryWriter(_writeStream);
            base.Initialize();
        }

        public void LoadContent()
        {
            _client = new TcpClient();
            _client.NoDelay = true;
            _client.Connect(IP, PORT);
            _readBuffer = new byte[BUFFER_SIZE];
            _client.GetStream().BeginRead(_readBuffer, 0, BUFFER_SIZE, StreamReceived, null);
            tankDataHolder = new TankDataHolder();
        }


        private void StreamReceived(IAsyncResult ar)
        {
            int bytesRead = 0;

            try
            {
                lock (_client.GetStream())
                {
                    // Get amount of bytes we can read.
                    bytesRead = _client.GetStream().EndRead(ar);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // Dont read if we dont have anything to read
            if (bytesRead == 0)
            {
                _client.Close();
                return;
            }

            byte[] data = new byte[bytesRead];

            for (int i = 0; i < bytesRead; ++i)
            {
                data[i] = _readBuffer[i];
            }

            ProcessData(data);

            _client.GetStream().BeginRead(_readBuffer, 0, BUFFER_SIZE, StreamReceived, null);
        }


        public void Update()
        {

        }

        private void ProcessData(byte[] data)
        {
            _readStream.SetLength(0);
            _readStream.Position = 0;

            _readStream.Write(data, 0, data.Length);
            _readStream.Position = 0;

            Protocol protocol;
            try
            {
                protocol = (Protocol)_reader.ReadByte();

                if (protocol == Protocol.Connected)
                {
                    byte ID = _reader.ReadByte();
                    String IP = _reader.ReadString();
                    if (!enemyConnected)
                    {
                        enemyConnected = true;
                        enemyCreated = true;
                        _writeStream.Position = 0;
                        _writer.Write((byte)Protocol.Connected);
                        SendData(GetDataFromMemoryStream(_writeStream));
                    }
                }
                else if (protocol == Protocol.Disconnected)
                {
                    PlayerDisconnected();
                }
                else if (protocol == Protocol.PlayerMoved)
                {

                    float px = _reader.ReadSingle();
                    float py = _reader.ReadSingle();
                    byte ID = _reader.ReadByte();
                    string IP = _reader.ReadString();

                    tankDataHolder.px = px;
                    tankDataHolder.py = py;

                }
                else if (protocol == Protocol.BulletCreated)
                {
                    // Create bullet.
                }
                else if (protocol == Protocol.PlayerRotated)
                {
                    Console.WriteLine(_readStream.ToString());

                    float angle = _reader.ReadSingle();
                    byte ID = _reader.ReadByte();
                    String IP = _reader.ReadString();

                    tankDataHolder.rotationAngle = angle;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void PlayerDisconnected()
        {
            byte ID = _reader.ReadByte();
            String IP = _reader.ReadString();
            enemyConnected = false;
        }

        public MemoryStream GetReadStream()
        {
            return _readStream;
        }

        public MemoryStream GetWriteStream()
        {
            return _writeStream;
        }

        public BinaryReader GetReader()
        {
            return _reader;
        }

        public BinaryWriter GetWriter()
        {
            return _writer;
        }

        public void SendData(byte[] data)
        {
            try
            {
                lock (_client.GetStream())
                {
                    _client.GetStream().BeginWrite(data, 0, data.Length, null, null);
                }
            }
            catch (System.Exception ex){}
        }

        public byte[] GetDataFromMemoryStream(MemoryStream ms)
        {
            byte[] result;

            //Async method called this, so lets lock the object to make 
            // sure other threads/async calls need to wait to use it.
            lock (ms)
            {
                int bytesWritten = (int)ms.Position;
                result = new byte[bytesWritten];

                ms.Position = 0;
                ms.Read(result, 0, bytesWritten);
            }

            return result;
        }
    }
}
