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
    public class Entity
    {
        public Vector2 pos;
        public float speed;
        public float rotation;
        public Vector2 look;
        public Texture2D sprite;

        private bool alive;

        public bool Alive
        {
            get { return alive; }
            set { alive = value; }
        }

        public virtual void Initialize()
        {
            pos.X = 100;
            pos.Y = 100;
            rotation = 0.0f;
            look.X = 0;
            look.Y = -1;
            alive = true;
        }
        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

    }
}
