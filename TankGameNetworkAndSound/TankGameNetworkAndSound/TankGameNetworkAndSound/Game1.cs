using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TankGameNetworkAndSound.Network;
using TankGameNetworkAndSound.Input;
using TankGameNetworkAndSound.GameObjects;

namespace TankGameNetworkAndSound
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        NetworkManager networkManager;
        InputManager inputManager;

        public PlayerConnectedEvent playerEvent { get; set; }

        Tank player;
        Tank enemy;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1080;
            graphics.PreferredBackBufferHeight = 720;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            networkManager = new NetworkManager(this);
            networkManager.Initialize();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            networkManager.LoadContent();

            inputManager = new InputManager(this);
            Services.AddService(typeof(IInputService), inputManager);
            Components.Add(inputManager);

            player = new Tank(this);
            player.LoadContent();

            enemy = null;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (networkManager.enemyCreated)
            {
                enemy = new Tank(this);
                enemy.position = new Vector2(100, 100);
                enemy.LoadContent();
                networkManager.enemyCreated = false;
            }

            if (enemy != null)
            {
                float px = networkManager.tankDataHolder.px;
                float py = networkManager.tankDataHolder.py;
                float angle = networkManager.tankDataHolder.rotationAngle;

                float newX = enemy.position.X + px;
                float newY = enemy.position.X + py;
                enemy.position = new Vector2(px,py);
                enemy.rotationAngle = angle;
            }
            
            player.Update(deltaTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (player != null) 
                player.Draw(spriteBatch);
            if(enemy != null)
                enemy.Draw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
