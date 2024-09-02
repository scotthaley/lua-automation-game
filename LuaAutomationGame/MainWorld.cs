﻿using System;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using LuaAutomationGame.Components.GameEngine;
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
    private readonly ISystem<float> _scriptSystem;
    private readonly World _world;

    public MainWorld(GraphicsDevice graphicsDevice, ContentManager content)
    {
        _graphicsDevice = graphicsDevice;

        _world = new World();

        _camera = new OrthographicCamera(graphicsDevice);

        _mainDrawSystem = new SequentialSystem<SpriteBatch>(new SpriteRenderSystem(_world, content));
        _scriptSystem = new SequentialSystem<float>(new ScriptablesSystem(_world));
    }

    public void Dispose()
    {
        _mainDrawSystem.Dispose();
        _world.Dispose();
    }

    public void Initialize()
    {
        var testDrone = _world.CreateEntity();
        testDrone.Set(new TransformComponent { Position = new Vector2(100, 100) });
        testDrone.Set(new SpriteComponent { TextureName = "Sprites/drone" });
        testDrone.Set(new ScriptableComponent
        {
            ScriptText = """
                         local timer = 0

                         function update(delta)
                            timer = timer + delta
                            
                            if (timer > 3) then
                                timer = 0
                                navigate(math.random(100, 800), math.random(100, 800))
                            end
                         end
                         """
        });
    }

    public void Update(float deltaTime)
    {
        _scriptSystem.Update(deltaTime);
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