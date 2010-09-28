using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Angels_Vs_Demons.Networking
{
    class networkUnit : Unit
    {
        int screenWidth = 853;
        int screenHeight = 480;
        public Vector2 Velocity;

        Vector2 screenSize;
        public Vector2 unitInput;

        public networkUnit(Texture2D loadedTexture, int gamerIndex, ContentManager manager)
            : base(loadedTexture)
        {
            position.X = screenWidth / 4 + (gamerIndex % 5) * screenWidth / 8;
            position.Y = screenHeight / 4 + (gamerIndex / 5) * screenHeight / 5;

            screenSize = new Vector2(screenWidth, screenHeight);
        }

        /// <summary>
        /// Draws the unit.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(sprite.Width / 2, sprite.Height / 2);

            spriteBatch.Draw(sprite, position, null, Color.White,
                             0, origin, 1, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Moves the unit in response to the current input settings.
        /// </summary>
        public void Update()
        {
            // If we have finished turning, also start moving forward.
            Velocity += unitInput;

            // Update the position and velocity.
            position += Velocity;

            // Clamp so the unit cannot drive off the edge of the screen.
            position = Vector2.Clamp(position, Vector2.Zero, screenSize);
        }
    }
}
