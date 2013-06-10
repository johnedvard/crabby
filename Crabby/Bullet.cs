using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Crabby
{
    class Bullet : MyGameObject
    {
        private int velocity = 15;

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Position.Y -= (int)velocity;
        }
    }
}
