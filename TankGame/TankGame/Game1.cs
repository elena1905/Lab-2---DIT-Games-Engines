/*
 * Game1.cs
 * 
 * Author: Elena Chen
 * Date: Oct 1st, 2012
 */

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

namespace TankGame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Instance;
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;
        public List<Entity> children = new List<Entity>();

        Tank playerTank;
        AITank enemyTank;

        Vector2 basis;
        Vector2 target;

        public int ScreenWidth
        {
            get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; }
        }

        public int ScreenHeight
        {
            get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; }
        }

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            basis = new Vector2(0, -1);

            playerTank = new Tank();
            enemyTank = new AITank();

            children.Add(playerTank);
            children.Add(enemyTank);
            
            for ( int i = 0; i < children.Count(); i++ )
            {
                children[i].Initialize();
            }

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

            for ( int i = 0; i < children.Count(); i++ )
            {
                children[i].LoadContent();
            }
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
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            for ( int i = 0; i < children.Count(); i++ )
            {
                children[i].Update(gameTime);

                // If an entity is dead, remove it from the children list
                if (!children[i].Alive)
                {
                    children.Remove(children[i]);
                }
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // AITank takes actions when playerTank is less than 100 close
            if (Detect())
            {
                target = playerTank.pos - enemyTank.pos;

                float theta = (float)Math.Acos(Vector2.Dot(basis, target) / target.Length());

                if (target.X < 0)
                {
                    enemyTank.rotation = (float)Math.PI * 2.0f - theta;
                }
                else
                {
                    enemyTank.rotation = theta;
                }

                enemyTank.Attack = true;

                if (Vector2.Distance(playerTank.pos, enemyTank.pos) < 50)
                {
                    enemyTank.pos -= enemyTank.look * 10;
                }
            }
            else
            {
                //enemyTank.rotation = 0.0f;
                enemyTank.Attack = false;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            for (int i = 0; i < children.Count; i++ )
            {
                children[i].Draw(gameTime);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        // For AITank to detect if playerTank is in shooting area (distance < 100)
        protected bool Detect()
        {
            float distance = Vector2.Distance(playerTank.pos, enemyTank.pos);

            if (distance > 200)
            {
                return false;
            }

            return true;
        }

        // Collision Detect
        protected bool Collide()
        {
            Rectangle rectPTank = new Rectangle((int)playerTank.pos.X,
                (int)playerTank.pos.Y,
                playerTank.sprite.Width,
                playerTank.sprite.Height);
            Rectangle rectETank = new Rectangle((int)enemyTank.pos.X,
                (int)enemyTank.pos.Y,
                enemyTank.sprite.Width + 200,
                enemyTank.sprite.Height + 200);

            return rectETank.Intersects(rectPTank);
        }
    }
}
