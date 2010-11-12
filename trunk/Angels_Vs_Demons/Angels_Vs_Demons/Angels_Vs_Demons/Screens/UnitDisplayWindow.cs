#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Angels_Vs_Demons.GameObjects.Units;
using Angels_Vs_Demons.Screens.ScreenManagers;
using Angels_Vs_Demons.Screens.GameplayScreens;
#endregion

namespace Angels_Vs_Demons.Screens
{
    class UnitDisplayWindow : GameplayScreen
    {
        #region Fields
        Vector2 size;
        List<String> fields = new List<String>();
        string menuTitle;
        SpriteFont font;
        Unit displayedUnit;

        #endregion

        #region Properties

        public int HP
        {
            set { HP = value; }
        }

        public Unit DisplayedUnit
        {
            get { return displayedUnit; }
            set { displayedUnit = value; }
        }

        #endregion

        #region Initialization

        public UnitDisplayWindow()
        {
            size.X = ScreenManager.screenWidth;
            size.Y = 200;

            String Name = "Unit: ";
            String HP = "HP: ";
            String Recharge = "Recharge: ";
            String Armour = "Armour: ";
            String AP = "AP: ";
            String MOV = "MOV: ";

            fields.Add(Name);
            fields.Add(HP);
            fields.Add(Recharge);
            fields.Add(Armour);
            fields.Add(AP);
            fields.Add(MOV);
            
        }

        #endregion

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            font = content.Load<SpriteFont>("DisplayFont");
        }

        #region Draw and Update

        /// <summary>
        /// Draws display window.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.AliceBlue, 0, 0);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            //SpriteFont font = ScreenManager.Font;

            Vector2 position = new Vector2(100, 350);

            spriteBatch.Begin();

            // Draw the unit display window.
            Vector2 titlePosition = new Vector2(426, 80);
            Vector2 titleOrigin = new Vector2(10, 500);
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            float titleScale = 1.25f;

            spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                   titleOrigin, titleScale, SpriteEffects.None, 0);

            spriteBatch.End();

        #endregion
        }
    }
}
