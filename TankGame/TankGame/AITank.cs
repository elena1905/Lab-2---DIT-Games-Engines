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
    class AITank : Tank
    {
        private bool attack;

        public bool Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public override void Initialize()
        {
            base.Initialize();
            pos.X = 200;
            pos.Y = 400;
            attack = false;
        }

        public override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!Attack)
            {   // Not attacked: AI movement
                look.X = (float)-Math.Cos(rotation);
                look.Y = (float)Math.Sin(rotation);

                pos += look * speed * timeDelta;

                // Window boundaries detect
                if (pos.X < sprite.Width / 2)
                {
                    rotation = MathHelper.Pi;
                }
                if (pos.X > Game1.Instance.Window.ClientBounds.Width - sprite.Width / 2)
                {
                    rotation = 0.0f;
                }
                if (pos.Y < sprite.Height / 2)
                {
                    pos.Y += speed * timeDelta;
                }
                if (pos.Y > Game1.Instance.Window.ClientBounds.Height - sprite.Height / 2)
                {
                    pos.Y -= speed * timeDelta;
                }
            }
            else
            {   // Attacked: move towards playerTank and fire bullets
                look.X = (float)Math.Sin(rotation);
                look.Y = (float)-Math.Cos(rotation);

                pos += look * speed * timeDelta;

                if (elapsedTime > (1.0f / fireRate))
                {
                    fireBullet();
                    elapsedTime = 0;
                }

                elapsedTime += timeDelta;

                if (elapsedTime >= 100)
                {
                    elapsedTime = 100;
                }
            }

        }

        // Method to fire a bullet
        private void fireBullet()
        {
            Bullet bullet = new Bullet();

            // Why load content explicitly???
            bullet.LoadContent();

            // Set bullet position where it should be fired
            bullet.pos = pos + look * (sprite.Height / 2);

            // Set direction at which bullet should be fired
            bullet.look = look;

            Game1.Instance.children.Add(bullet);
        }
    }
}
