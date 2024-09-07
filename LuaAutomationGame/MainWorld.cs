using System;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using LuaAutomationGame.Components.GameEngine;
using LuaAutomationGame.Items.Ore;
using LuaAutomationGame.Systems.GameSystems;
using LuaAutomationGame.Systems.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace LuaAutomationGame;

public class MainWorld : IDisposable
{
    private readonly OrthographicCamera _camera;
    private readonly GraphicsDevice _graphicsDevice;

    // systems
    private readonly ISystem<SpriteBatch> _mainDrawSystem;
    private readonly ISystem<float> _mainGameSystem;
    private readonly ISystem<float> _scriptSystem;
    private readonly World _world;

    public MainWorld(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _graphicsDevice = graphicsDevice;

        _world = new World();

        _camera = new OrthographicCamera(graphicsDevice);

        _mainDrawSystem = new SequentialSystem<SpriteBatch>(new SpriteRenderSystem(_world, content));
        _scriptSystem = new SequentialSystem<float>(new ScriptablesSystem(_world));
        _mainGameSystem = new SequentialSystem<float>(
            new NavigationSystem(_world),
            new GridPositionSystem(_world),
            new InventorySystem(_world)
        );
    }

    public void Dispose()
    {
        _mainDrawSystem.Dispose();
        _scriptSystem.Dispose();
        _mainGameSystem.Dispose();
        _world.Dispose();
    }

    public void Initialize()
    {
        var testMine = _world.CreateEntity();
        testMine.Set(new TransformComponent());
        testMine.Set(new GridPositionComponent { X = 10, Y = 8 });
        testMine.Set(new SpriteComponent { TextureName = "Sprites/mine-placeholder" });
        testMine.Set(new InventoryComponent
        {
            Items = [new ItemIronOre(64)],
            MaxItems = 6
        });

        var testDrone = _world.CreateEntity();
        testDrone.Set(new TransformComponent());
        testDrone.Set(new GridPositionComponent());
        testDrone.Set(new SpriteComponent { TextureName = "Sprites/drone" });
        testDrone.Set(new NavigationComponent());
        testDrone.Set(new InventoryComponent(6));
        testDrone.Set(new ScriptableComponent
        {
            ScriptText = """
                         navigate(10, 8)

                         function update(delta)
                            if (not is_navigating() and not is_extracting()) then
                                extract()
                            end
                         end
                         """
        });
    }

    public void Update(float deltaTime)
    {
        _scriptSystem.Update(deltaTime);
        _mainGameSystem.Update(deltaTime);
    }

    public void Draw(SpriteBatch spriteBatch, float deltaTime)
    {
        _graphicsDevice.Clear(Color.CornflowerBlue);

        var transformMatrix = _camera.GetViewMatrix();

        spriteBatch.Begin(transformMatrix: transformMatrix);
        _mainDrawSystem.Update(spriteBatch);
        spriteBatch.End();
    }
}