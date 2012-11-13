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
    class Bullet : Entity
    {
        Vector2 center;

        public override void LoadContent()
        {
            //base.LoadContent();
            speed = 200.0f;
            sprite = Game1.Instance.Content.Load<Texture2D>("bullet");
            center.X = sprite.Width / 2;
            center.Y = sprite.Height / 2;
            Alive = true;
        }

        public override void Update(GameTime gameTime) 
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //speed = 100.0f;

            pos += look * speed * timeDelta;

            // Bounds checking
            if (pos.X < 0 ||
                pos.Y < 0 ||
                pos.X > Game1.Instance.Window.ClientBounds.Width - sprite.Width / 2 ||
                pos.Y > Game1.Instance.Window.ClientBounds.Height - sprite.Height / 2)
            {
                Alive = false;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game1.Instance.spriteBatch.Draw(sprite, pos, null, Color.White, rotation, center, Vector2.One, SpriteEffects.None, 0);
        }
    }
}