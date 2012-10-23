using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TankGameNetworkAndSound.Input
{
    /// <summary>
    /// Implements the IInputService interface and the GameComponent class.
    /// For information about functions, read comments in IInputService.
    /// </summary>
    public class InputManager : GameComponent, IInputService
    {

        #region MouseProperties
        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(currentMouseState.X, currentMouseState.Y);
            }
        }

        public Vector2 MousePreviousPosition
        {
            get
            {
                return new Vector2(previousMouseState.X, previousMouseState.Y);
            }
        }

        public int ScrollWheelChange
        {
            get
            {
                return previousMouseState.ScrollWheelValue - currentMouseState.ScrollWheelValue;
            }
        }

        public int ScrollWheelTotal
        {
            get
            {
                return currentMouseState.ScrollWheelValue;
            }
        }
        #endregion

        private MouseState previousMouseState;
        private MouseState currentMouseState;
        private KeyboardState previousKeyboardState;
        private KeyboardState currentKeyboardState;

        public InputManager(Game game) : 
            base(game)
        {
            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();
            previousMouseState = currentMouseState;
            previousKeyboardState = currentKeyboardState;
        }

        public override void Update(GameTime gameTime)
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();
            base.Update(gameTime);
        }

        public bool IsButtonDown(MouseButtons button)
        {
            switch (button)
            {
                case(MouseButtons.leftButton):
                    {
                        if(currentMouseState.LeftButton == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case(MouseButtons.rightButton):
                    {
                        if(currentMouseState.RightButton == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case(MouseButtons.middleButton):
                    {
                        if(currentMouseState.MiddleButton == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case(MouseButtons.xButtoneOne):
                    {
                        if(currentMouseState.XButton1 == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case(MouseButtons.xButtonTwo):
                    {
                        if(currentMouseState.XButton2 == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        public bool IsButtonUp(MouseButtons button)
        {
            switch (button)
            {
                case (MouseButtons.leftButton):
                    {
                        if (currentMouseState.LeftButton == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.rightButton):
                    {
                        if (currentMouseState.RightButton == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.middleButton):
                    {
                        if (currentMouseState.MiddleButton == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.xButtoneOne):
                    {
                        if (currentMouseState.XButton1 == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.xButtonTwo):
                    {
                        if (currentMouseState.XButton2 == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
            }

            return false;

        }

        public bool IsButtonPressed(MouseButtons button)
        {
            switch (button)
            {
                case (MouseButtons.leftButton):
                    {
                        if (currentMouseState.LeftButton == ButtonState.Pressed 
                            && previousMouseState.LeftButton == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.rightButton):
                    {
                        if (currentMouseState.RightButton == ButtonState.Pressed
                            && previousMouseState.RightButton == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.middleButton):
                    {
                        if (currentMouseState.MiddleButton == ButtonState.Pressed
                            && previousMouseState.MiddleButton == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.xButtoneOne):
                    {
                        if (currentMouseState.XButton1 == ButtonState.Pressed
                            && previousMouseState.XButton1 == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.xButtonTwo):
                    {
                        if (currentMouseState.XButton2 == ButtonState.Pressed
                            &&previousMouseState.XButton2 == ButtonState.Released)
                        {
                            return true;
                        }
                        break;
                    }
            }

            return false;

        }

        public bool IsButtonReleased(MouseButtons button)
        {
            switch (button)
            {
                case (MouseButtons.leftButton):
                    {
                        if (currentMouseState.LeftButton == ButtonState.Released
                            && previousMouseState.LeftButton == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.rightButton):
                    {
                        if (currentMouseState.RightButton == ButtonState.Released
                            && previousMouseState.RightButton == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.middleButton):
                    {
                        if (currentMouseState.MiddleButton == ButtonState.Released
                            && previousMouseState.MiddleButton == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.xButtoneOne):
                    {
                        if (currentMouseState.XButton1 == ButtonState.Released
                            && previousMouseState.XButton1 == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
                case (MouseButtons.xButtonTwo):
                    {
                        if (currentMouseState.XButton2 == ButtonState.Released
                            && previousMouseState.XButton2 == ButtonState.Pressed)
                        {
                            return true;
                        }
                        break;
                    }
            }

            return false;
        }

        public bool IsKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyPressed(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return currentKeyboardState.IsKeyUp(key) && previousKeyboardState.IsKeyDown(key);
        }
    }
}
