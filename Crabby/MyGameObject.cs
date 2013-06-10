using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Crabby
{
    public class MyGameObject
    {
        public int gravity = 0;
        public Vector2 Position;
        public Texture2D Texture;
        public bool IsNonExisting = false;
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }

        public virtual void Move(Vector2 direction)
        {
            Position.X += direction.X;
        }
        public virtual void Update(GameTime gameTime)
        {

        }
        public Rectangle BoundingBox
        {
            get 
            { 
                return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height); 
            }
        }
    }
}
