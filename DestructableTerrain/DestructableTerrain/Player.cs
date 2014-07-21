using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DestructableTerrain
{
    public class Player
    {
        public static Rectangle rectangle;
        public static Vector2 velocity = Vector2.Zero;

        public static int X
        {
            get
            {
                return rectangle.X;
            }
            set
            {
                rectangle.X = value;
            }
        }
        public static int Y
        {
            get
            {
                return rectangle.Y;
            }
            set
            {
                rectangle.Y = value;
            }
        }

        public static void Update()
        {
            //velocity.Y += 0.5f;
            //if (Player.velocity.Y > 20) velocity.Y = 20;

            if (Main.ks.IsKeyDown(Keys.A))
            {
                velocity.X = -1;
            }
            else if (Main.ks.IsKeyDown(Keys.D))
            {
                velocity.X = 1;
            }
            if (Main.ks.IsKeyDown(Keys.W))
            {
                velocity.Y = -1;
            }
            else if (Main.ks.IsKeyDown(Keys.S))
            {
                velocity.Y = 1;
            }
            rectangle.X += (int) velocity.X;
            rectangle.Y += (int) velocity.Y;

            velocity.X = MathHelper.Lerp(velocity.X, 0, 0.3f);
            if (velocity.X <= 1f) velocity.X = 0f;
            velocity.Y = MathHelper.Lerp(velocity.Y, 0, 0.3f);
            if (velocity.Y <= 1f) velocity.Y = 0f;


            Texture2D t = Main.terrain[Main.curTerrain];
            Color[] terrainData = new Color[t.Width * t.Height];
            t.GetData<Color>(terrainData, 0, t.Width * t.Height);

            Color[] charTextureData = new Color[Main.characterTexture.Width * Main.characterTexture.Height];
            Main.characterTexture.GetData<Color>(charTextureData, 0, rectangle.Width * rectangle.Height);

            bool c = false;
            for (int X = 0; X < rectangle.Width; X++)
            {
                for (int Y = 0; Y < rectangle.Height; Y++)
                {
                    if (((rectangle.X + X) < (t.Width)) && ((rectangle.Y + Y) < (t.Height)))
                    {
                        if ((rectangle.X + X) >= 0 && (rectangle.Y + Y) >= 0)
                        {
                            if (terrainData[(X + rectangle.X) + (Y + rectangle.Y) * t.Width] != Main.emptyColor // map is collidable
                                && charTextureData[X+Y * rectangle.Width] != Main.emptyColor) // player is collidable
                            {
                                if (Main.destroy)
                                {
                                    terrainData[(X + rectangle.X) + (Y + rectangle.Y) * t.Width] = Main.emptyColor;
                                }
                                else
                                {
                                    int pX = X + rectangle.X, pY = Y + rectangle.Y; // pixel on player in question relative to terrain

                                    if (pX < rectangle.X+32) // if pixel is on top of player
                                    {
                                        
                                    }

                                }
                                c = true;
                            }
                        }
                    }
                }
            }
            if (c) Main.bgColor = Color.Red; else Main.bgColor = Color.Green;

            Main.terrain[Main.curTerrain].SetData<Color>(terrainData);
        }
    }
}
