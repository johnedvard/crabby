using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Crabby
{
  
    class Player : MyGameObject
    {
        double jumpForce = 0;
        public Player()
        {
            gravity = -8;
        }
     
        public bool CheckWallCollision()
        {
            bool didCollide = false;
            if (Position.X < 0)
            {
                 Position.X = 0;
                 didCollide = true;
            }
            if (Position.X + Texture.Width > Game1.screenWidth)
            {
                Position.X = Game1.screenWidth - Texture.Width;
                didCollide = true;
            }
            return didCollide;
        }
        public void Jump(bool doJump)
        {
            if (doJump && Position.Y >= Game1.screenHeight - Game1.DRAG_OFFSET - Texture.Height)
            {
                jumpForce = 25;
            }
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (jumpForce >= 0)
            {
                jumpForce--;
            }
            Position.Y -= gravity + (int)jumpForce;
            if (Position.Y >= Game1.screenHeight - Game1.DRAG_OFFSET - Texture.Height)
                Position.Y = Game1.screenHeight - Game1.DRAG_OFFSET - Texture.Height;
            
        }
        public override void Move(Vector2 distance)
        {
            base.Move(distance);
            
            CheckWallCollision();
        }
    }
}
