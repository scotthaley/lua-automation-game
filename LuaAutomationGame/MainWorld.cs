using System;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using LuaAutomationGame.Systems.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LuaAutomationGame;

public class MainWorld : IDisposable
{
    private GraphicsDevice _graphicsDevice;
    private readonly World _world;
    private readonly OrthographicCamera _camera;
    
    // systems
    private readonly ISystem<SpriteBatch> _mainDrawSystem;
    
    public MainWorld(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _graphicsDevice = graphicsDevice;
        
        _world = new World();
        
        _camera = new OrthographicCamera(graphicsDevice);
        
        _mainDrawSystem = new SequentialSystem<SpriteBatch>(new SpriteRenderSystem(_world, content));
    }
    
    public void Initialize()
    {
        var testDrone = _world.CreateEntity();
        testDrone.Set(new TransformComponent { Position = new Vector2(100, 100) });
        testDrone.Set(new SpriteComponent { TextureName = "Sprites/drone" });
    }
    
    public void Update(float deltaTime)
    {
    }
    
    public void Draw(SpriteBatch spriteBatch, float deltaTime)
    {
        _graphicsDevice.Clear(Color.CornflowerBlue);
        
        var transformMatrix = _camera.GetViewMatrix();
        
        spriteBatch.Begin(transformMatrix: transformMatrix);
        _mainDrawSystem.Update(spriteBatch);
        spriteBatch.End();
    }
    
    public void Dispose()
    {
        _mainDrawSystem.Dispose();
        _world.Dispose();
    }
}