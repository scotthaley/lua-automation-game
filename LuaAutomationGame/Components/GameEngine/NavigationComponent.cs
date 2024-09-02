using Microsoft.Xna.Framework;

namespace LuaAutomationGame.Components.GameEngine;

public struct NavigationComponent
{
    public Vector2? Target { get; set; }
    public float Speed { get; set; }
    public float TopSpeed { get; set; }
    public float Acceleration { get; set; }

    public NavigationComponent()
    {
        Target = null;
        Speed = 0;
        Acceleration = 100;
        TopSpeed = 5000;
    }
}