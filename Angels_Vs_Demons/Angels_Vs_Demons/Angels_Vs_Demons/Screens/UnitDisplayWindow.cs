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
    class UnitDisplayWindow : GameScreen
    {
        ContentManager content;
        SpriteBatch spritebatch;
        #region Fields
        SpriteFont font;
        Unit displayedUnit;
        Vector2 textPosition;
        Vector2 textOrigin;
        Color titleColor;
        float titleScale;
        String Name, HP, Recharge, Armor, Movement, Attack, Power, Range;

        #endregion

        #region Properties

        public UnitDisplayWindow(ContentManager Content)
        {
            content = Content;
            font = content.Load<SpriteFont>("MenuFont");

            textPosition = new Vector2(100, 400);
            textOrigin = new Vector2(100, 400);
            titleColor = new Color(0, 0, 0, 255);
            titleScale = 0.75f;

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
            
            
        }

        #endregion

        #region Draw and Update

        /// <summary>
        /// Draws display window.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Unit currentTileUnit)
        {
            //SpriteFont font = ScreenManager.Font;
            if (currentTileUnit != null)
            {
                displayedUnit = currentTileUnit;
                Name = "Name: " + currentTileUnit.Name;
                HP = "HP: " + currentTileUnit.CurrHP + "/" + currentTileUnit.TotalHP;
                Recharge = "Recharge: " + currentTileUnit.CurrRecharge + "/" + currentTileUnit.TotalRecharge;
                Armor = "Armor: " + currentTileUnit.Armor;
                Movement = "Movement: " + currentTileUnit.Movement;
                if (currentTileUnit is NonChampion)
                {
                    NonChampion nc = currentTileUnit as NonChampion;
                    Attack = "Attack: " + nc.AttackTypeVal;
                    Power = "Power: " + nc.AttackPower;
                    Range = "Range: " + nc.Range;
                }
                else
                {
                    Attack = "Attack: --";
                    Power = "Power: --";
                    Range = "Range: --";
                }
                    
            }
            else
            {
                Name = "Name: ";
                HP = "HP: ";
                Recharge = "Recharge: ";
                Armor = "Armor: ";
                Movement = "Movement: ";
                Attack = "Attack: ";
                Power = "Power: ";
                Range = "Range: ";
            }
            
            spritebatch = spriteBatch;

            spritebatch.Begin();

            textPosition.Y = 1470;
            textOrigin.Y = 1470;
            textPosition.X = 950;
            textOrigin.X = 950;

            spritebatch.DrawString(font, Name, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, HP, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Recharge, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Movement, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y = 1470;
            textOrigin.Y = 1470;
            textPosition.X += 950;
            textOrigin.X += 950;

            spritebatch.DrawString(font, Armor, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Attack, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Power, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            textPosition.Y += font.LineSpacing;

            spritebatch.DrawString(font, Range, textPosition, titleColor, 0,
                                   textOrigin, titleScale, SpriteEffects.None, 0);

            spritebatch.End();

        #endregion
        }
    }
}
