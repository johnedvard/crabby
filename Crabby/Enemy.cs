using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crabby
{
    class Enemy : MyGameObject
    {
        public int direction = 0;
        public int velocity = 0;
        private EnemyType type = EnemyType.BIG_FISH;
        public Enemy(EnemyType type, int yPos,Texture2D texture)
        {
            Texture = texture;
            Type = type;
            if (type == EnemyType.BIG_FISH)
            {
                Position = new Vector2(Game1.screenWidth, yPos);
                direction = -1;
                velocity = 5;
            }
            else if (type == EnemyType.SMALL_FISH)
            {
                Position = new Vector2(0-texture.Width, yPos);
                direction = 1;
                velocity = 3;
            }
        }
        public void Decend()
        {
            gravity = 5;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position.X += direction * velocity;
            Position.Y += Math.Abs(direction) * gravity*2;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            float angle = 0;
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(0, 0);
            Vector2 scale = new Vector2(1, 1);

            if (gravity > 0)
                angle = 0.5f;
            if (direction == -1)
                spriteBatch.Draw(Texture,Position,sourceRectangle,Color.White,6.24f-angle,origin,scale,SpriteEffects.FlipHorizontally,1);
            else
                spriteBatch.Draw(Texture, Position, sourceRectangle, Color.White, angle, origin, scale, SpriteEffects.None, 1);
            
        }
        public EnemyType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

    }
}
