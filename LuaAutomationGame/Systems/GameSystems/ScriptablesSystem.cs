using System;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.GameEngine;
using MoonSharp.Interpreter;

namespace LuaAutomationGame.Systems.GameSystems;

public class ScriptablesSystem(World world)
    : AEntitySetSystem<float>(world.GetEntities().With<ScriptableComponent>().AsSet())
{
    protected override void Update(float state, in Entity entity)
    {
        var scriptText = entity.Get<ScriptableComponent>().Script;
        
        var script = new Script();
        script.Globals["print"] = (Func<string, bool>)Log;
        script.DoString(scriptText);

        script.Call(script.Globals["update"]);
    }
    
    private static bool Log(string message)
    {
        Console.WriteLine(message);
        return true;
    }
}