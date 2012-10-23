using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TankGameNetworkAndSound.Input;
using Microsoft.Xna.Framework.Input;
using TankGameNetworkAndSound.Network;

namespace TankGameNetworkAndSound.GameObjects
{
    public class Tank
    {
        private const float ROTATION_SPEED = 3.0f;
        private const float ACCELERATION = 8.5f;
        private const float FRICTION = 1.9f;

        private Game _game;

        // Physics
        private Texture2D _bodyTexture;
        public Vector2 position { get; set; }      // position on screen
        private Vector2 _origin;        // local origin position.
        private Vector2 _direction;
        public Vector2 velocity { get; set; }
        public float rotationAngle { get; set; }

        private IInputService _input;
        private INetworkService _network;

        public Tank(Game game)
        {
            _game = game;
            _input = (IInputService)_game.Services.GetService(typeof(IInputService));
            _network = (INetworkService)_game.Services.GetService(typeof(INetworkService));
            velocity = Vector2.Zero;
            _direction = Vector2.Zero;
            position = new Vector2(100, 100);
            _origin = Vector2.Zero;
            rotationAngle = (float)Math.PI / 2;
        }

        public void LoadContent()
        {
            _bodyTexture = _game.Content.Load<Texture2D>("tankBody");
            if (_bodyTexture == null) { Console.WriteLine("No image found"); }
            _origin = new Vector2(_bodyTexture.Width / 2, _bodyTexture.Height / 2);
            SendPositionData();
        }

        public void Update(float deltaTime)
        {
            HandleInput(deltaTime);
            HandleMovement(deltaTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_bodyTexture, position, null, Color.White,
                rotationAngle, _origin, 1, SpriteEffects.None, 0);
        }


        public void HandleInput(float deltaTime)
        {
            float prevAngle = rotationAngle;

            if (_input.IsKeyDown(Keys.D))
            {
                rotationAngle += deltaTime * ROTATION_SPEED;
            }
            if (_input.IsKeyDown(Keys.A))
            {
                rotationAngle += deltaTime * -ROTATION_SPEED;
            }
            float currentAngle = rotationAngle;

            if ((prevAngle - rotationAngle) != 0)
            {
                _network.GetWriteStream().Position = 0;
                _network.GetWriter().Write((byte)Protocol.PlayerRotated);
                _network.GetWriter().Write(rotationAngle);
                _network.SendData(_network.GetDataFromMemoryStream(_network.GetWriteStream()));
            }

            if (_input.IsKeyDown(Keys.W))
            {
                velocity += new Vector2(1, 1) * ACCELERATION * deltaTime;
            }
            if (_input.IsKeyDown(Keys.S))
            {
                velocity += new Vector2(1, 1) * -ACCELERATION * deltaTime;
            }
        }

        private void HandleMovement(float deltaTime)
        {
            Vector2 iPos = new Vector2(position.X, position.Y);
            
            velocity -= velocity * FRICTION * deltaTime;
            float newDirX = (float)Math.Cos(rotationAngle);
            float newDirY = (float)Math.Sin(rotationAngle);
            _direction = new Vector2(newDirX, newDirY);
            position += velocity * Vector2.Normalize(_direction);

            Vector2 nPos = new Vector2(position.X, position.Y);
            Vector2 deltaPos = nPos - iPos;

            if (deltaPos != Vector2.Zero)
            {
                SendPositionData();
            }
        }

        private void SendPositionData()
        {
            _network.GetWriteStream().Position = 0;
            _network.GetWriter().Write((byte)Protocol.PlayerMoved);
            _network.GetWriter().Write(position.X);
            _network.GetWriter().Write(position.Y);
            _network.SendData(_network.GetDataFromMemoryStream(_network.GetWriteStream()));
        }
    }
}
