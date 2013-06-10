using System;
using System.Collections.Generic;
using Microsoft.Devices.Sensors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Microsoft.Xna.Framework.Input.Touch;
namespace Crabby
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        public const int DRAG_OFFSET = 170; // used to make space under the crab
        private bool _firstUpdate = true; // used for fixing touch gestures **HACK**
        private int highScore = 0;
        private int prevBestScore = 0;
        string randomText;
        Texture2D textureBullet;
        Texture2D textureBigFish;
        Texture2D textureSmallFish;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        public static int screenWidth;
        public static int screenHeight;
        MyGameObject player;
        List<MyGameObject> gameObjects = new List<MyGameObject>();
        Random random;
        ScoreBoard scoreBoard;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            TouchPanel.EnabledGestures = GestureType.FreeDrag | GestureType.Tap | GestureType.Flick;
            random = new Random();
            scoreBoard = new ScoreBoard();
            prevBestScore = scoreBoard.getScore("john");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            player = new Player();
            gameObjects.Add(player);
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            player.Texture = Content.Load<Texture2D>("Crabby_v2");
            textureBullet = Content.Load<Texture2D>("Crabby_claw_v2");
            textureSmallFish = Content.Load<Texture2D>("Fish_Small_v2");
            textureBigFish = Content.Load<Texture2D>("Fish_Big_v2");

            player.Position = new Vector2(screenWidth / 2 + player.Texture.Width/2, screenHeight - player.Texture.Height - DRAG_OFFSET);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            base.UnloadContent();
        }
       
        
        public void SpawnEnemy()
        {
            int randomNumber = random.Next(0, 1000);
            int limit = randomNumber % 200;
            if(limit > 195){
                MyGameObject e;
                if (limit < 197)
                {
                    e = new Enemy(EnemyType.BIG_FISH, random.Next(0,screenHeight - DRAG_OFFSET - player.Texture.Height),textureBigFish);
                }
                else
                {
                    e = new Enemy(EnemyType.SMALL_FISH, random.Next(0, screenHeight - DRAG_OFFSET - player.Texture.Height),textureSmallFish);
                }
                gameObjects.Add(e);
            }
        }
      
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            randomText ="elapsed time in millis: " + gameTime.ElapsedGameTime.Milliseconds;
            SpawnEnemy();
            FixTouchGestureHack();
           
            // TODO: Add your update logic here
            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;
            
            // handle player 
            Vector2 touchInputVelocity;
            bool isTappingGesture, isFlickingGesture;
            Input.ProcessTouchInput(out touchInputVelocity, out isTappingGesture, out isFlickingGesture);
            player.Move(touchInputVelocity);
            ((Player)player).Jump(isFlickingGesture);
            ShootBullet(isTappingGesture);
            

            // update all gameObjects
            foreach(MyGameObject o in gameObjects){
                o.Update(gameTime);
                CheckForCollisions(o);
            }
          
            base.Update(gameTime);
        }
        /// <summary>
        /// Check if the bullet is out of the screen or if it hit an enemy
        /// </summary>
        /// <param name="bullet"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CheckForBulletCollisions(MyGameObject bullet, MyGameObject other)
        {
            // remove bullet if it is out of screen
            if (bullet.Position.Y + bullet.Texture.Height <= 0)
            {
                bullet.IsNonExisting = true;
                return true;
            }
                // remove bullet if it hits an enemy
            if (other is Enemy && bullet.BoundingBox.Intersects(other.BoundingBox))
                {
                    bullet.IsNonExisting = true;
                    ((Enemy)other).Decend();
                    return true;
                }
            return false;
        }
        public void increaseScore(EnemyType enemyType)
        {
            if (enemyType.Equals(EnemyType.BIG_FISH))
                highScore += 1;
            else
                highScore += 3;

            ScoreBoard.score = highScore;
            if (highScore >= prevBestScore)
            {
                prevBestScore = highScore;
            }
        }
        /// <summary>
        /// Check if the player can eat an enemy
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
        public bool CheckForEnemyCollisions(MyGameObject enemy, MyGameObject player)
        {
            // check if enemy is out of the screen
            if (enemy.Position.X + enemy.Texture.Width*3 < 0  || enemy.Position.X >=screenWidth*2)
            {
                enemy.IsNonExisting = true;
                return true;
            }
            // check if enemy hits the player
            if (player is Player && player.BoundingBox.Intersects(enemy.BoundingBox))
            {
                increaseScore(((Enemy)enemy).Type);
                enemy.IsNonExisting = true;
                return true;
            }
            return false;
        }
        public void CheckForCollisions(MyGameObject o)
        {
            foreach (MyGameObject other in gameObjects)
            {
                if (o is Bullet && CheckForBulletCollisions(o, other))
                    break;
                else if( o is Enemy && CheckForEnemyCollisions(o, other))
                    break;
                
            }
        }
        public void ShootBullet(bool doShoot)
        {
            if (doShoot)
            {
                Bullet b = new Bullet();
                Vector2 playerPos = player.Position;
                b.Texture = textureBullet;
                b.Position.X = playerPos.X + player.Texture.Width / 2;
                b.Position.Y = playerPos.Y;
                gameObjects.Add(b);
            }
        }
        /// <summary>
        /// Take a look at this link. https://github.com/mono/MonoGame/issues/1046
        /// It might have a fix for not having to use this hack.
        /// </summary>
        public void FixTouchGestureHack()
        {
            if (_firstUpdate)
            {
                // Temp hack to fix gestures
                typeof(Microsoft.Xna.Framework.Input.Touch.TouchPanel)
                    .GetField("_touchScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .SetValue(null, Vector2.One);

                _firstUpdate = false;
            }
        }
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            List<MyGameObject> objectsToRemove = new List<MyGameObject>();
            _spriteBatch.Begin();
            foreach (MyGameObject o in gameObjects)
                if (o.IsNonExisting) objectsToRemove.Add(o);
                else o.Draw(_spriteBatch);
            _spriteBatch.End();
            // clean up missing objects
            foreach (MyGameObject o in objectsToRemove)
                gameObjects.Remove(o);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(Content.Load<SpriteFont>("MyFont"), "Score: " + highScore + " best score: " + prevBestScore, new Vector2(5, 5), Color.White);
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
