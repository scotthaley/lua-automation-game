using MoonSharp.Interpreter;

namespace LuaAutomationGame.Components.GameEngine;

public struct ScriptableComponent
{
    public string ScriptText { get; set; }
    public Script Script { get; set; }
}