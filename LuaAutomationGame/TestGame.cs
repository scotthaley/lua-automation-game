using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LuaAutomationGame;

public class TestGame : Game
{
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private readonly MainWorld _mainWorld;
    
        public TestGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
            _graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _mainWorld = new MainWorld(GraphicsDevice, Content);
        }
    
        protected override void Initialize()
        {
            _mainWorld.Initialize();
            base.Initialize();
        }
    
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
    
            // TODO: use this.Content to load your game content here
        }
    
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
    
            _mainWorld.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
    
            base.Update(gameTime);
        }
    
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
    
            _mainWorld.Draw(_spriteBatch, (float)gameTime.ElapsedGameTime.TotalSeconds);
    
            base.Draw(gameTime);
        }

        protected override void Dispose(bool disposing)
        {
            _graphics.Dispose();
            _mainWorld.Dispose();
            _spriteBatch.Dispose();
            
            base.Dispose(disposing);
        }
}