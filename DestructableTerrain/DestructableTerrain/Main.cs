using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace DestructableTerrain
{
    public class Main : Game
    {
        private static GraphicsDeviceManager graphics;
        private static SpriteBatch spriteBatch;

        public static int screenWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        public static int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public static int curTerrain = 0;
        public static Texture2D[] terrain = new Texture2D[4];
        public static Texture2D mouseIcon;
        public static Texture2D characterTexture;

        public static Color emptyColor = new Color(0, 0, 0, 0);

        public static bool destroy = false;

        public static MouseState ms = Mouse.GetState(), lastms = Mouse.GetState();
        public static KeyboardState ks = Keyboard.GetState(), lastks = Keyboard.GetState();

        public static Color bgColor = Color.Green;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            mouseIcon = Content.Load<Texture2D>("image/ui/mouse");
            for (int i = 0; i < Main.terrain.Length; i++) terrain[i] = Content.Load<Texture2D>("image/terrain/terrain_" + i);
            characterTexture = Content.Load<Texture2D>("image/entity/character");
            Player.rectangle = new Rectangle(0, 0, characterTexture.Width, characterTexture.Height);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            ks = Keyboard.GetState();
            ms = Mouse.GetState();

            destroy = ks.IsKeyDown(Keys.Space);

            if (ms.LeftButton == ButtonState.Pressed)
            {
                Explosion(ms.X, ms.Y, 64);
            }
            if (ms.RightButton == ButtonState.Pressed)
            {
                Paint(ms.X, ms.Y, 64, Color.Black);
            }

            if (ks.IsKeyDown(Keys.E) && lastks.IsKeyUp(Keys.E))
            {
                Player.X = ms.X-32;
                Player.Y = ms.Y-32;
            }

            if (ks.IsKeyDown(Keys.Left) && lastks.IsKeyUp(Keys.Left))
            {
                curTerrain--;
                if (curTerrain < 0) curTerrain = 0;
                Player.X = 0;
                Player.Y = 0;
            }
            else if (ks.IsKeyDown(Keys.Right) && lastks.IsKeyUp(Keys.Right))
            {
                curTerrain++;
                if (curTerrain >= terrain.Length) curTerrain = terrain.Length - 1;
                Player.X = 0;
                Player.Y = 0;
            }

            Player.Update();

            lastks = ks;
            lastms = ms;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(bgColor);

            spriteBatch.Begin();

            spriteBatch.Draw(terrain[curTerrain], new Vector2(0, 0), Color.White);
            spriteBatch.Draw(characterTexture, Player.rectangle, Color.White);
            spriteBatch.Draw(mouseIcon, new Vector2(ms.X, ms.Y), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // custom methods //

        public static void Explosion(int x, int y, int radius)
        {
            Texture2D t = terrain[curTerrain];
            Color[] terrainData = new Color[t.Width * t.Height];
            t.GetData<Color>(terrainData, 0, t.Width * t.Height);

            for (int X = -radius/2; X < radius/2; X++)
            {
                for (int Y = -radius/2; Y < radius/2; Y++)
                {
                    if (((x + X) < (t.Width)) && ((y + Y) < (t.Height)))
                    {
                        if ((x + X) >= 0 && (y + Y) >= 0)
                        {
                            terrainData[(X + x) + (Y + y) * t.Width] = Color.White * 0f;
                        }
                    }
                }
            }

            terrain[curTerrain].SetData<Color>(terrainData);
        }

        public static void Paint(int x, int y, int radius, Color color)
        {
            Texture2D t = terrain[curTerrain];
            Color[] terrainData = new Color[t.Width * t.Height];
            t.GetData<Color>(terrainData, 0, t.Width * t.Height);

            for (int X = -radius / 2; X < radius / 2; X++)
            {
                for (int Y = -radius / 2; Y < radius / 2; Y++)
                {
                    if (((x + X) < (t.Width)) && ((y + Y) < (t.Height)))
                    {
                        if ((x + X) >= 0 && (y + Y) >= 0)
                        {
                            terrainData[(X + x) + (Y + y) * t.Width] = color;
                        }
                    }
                }
            }

            terrain[curTerrain].SetData<Color>(terrainData);
        }
    }
}
