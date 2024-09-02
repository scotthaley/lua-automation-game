using System;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.GameEngine;
using Microsoft.Xna.Framework;
using MoonSharp.Interpreter;

namespace LuaAutomationGame.Systems.GameSystems;

public class ScriptablesSystem(World world)
    : AEntitySetSystem<float>(world.GetEntities().With<ScriptableComponent>().AsSet())
{
    protected override void Update(float state, in Entity entity)
    {
        var script = entity.Get<ScriptableComponent>().Script;
        if (script == null)
            try
            {
                script = new Script();
                var copiedEntity = entity;

                script.Globals["print"] = (Func<string, bool>)Log;
                script.Globals["navigate"] = (Func<int, int, bool>)((x, y) => Navigate(copiedEntity, x, y));
                script.Globals["is_navigating"] = (Func<bool>)(() => IsNavigating(copiedEntity));

                script.DoString(entity.Get<ScriptableComponent>().ScriptText);
                entity.Get<ScriptableComponent>().Script = script;
            }
            catch (SyntaxErrorException ex)
            {
                Console.WriteLine("Error in script: {0}", ex.DecoratedMessage);
            }
        else
            script.Call(script.Globals["update"], state);
    }

    private static bool Log(string message)
    {
        // TODO: Implement logging in game?
        Console.WriteLine(message);
        return true;
    }

    private static bool Navigate(Entity entity, int x, int y)
    {
        if (!entity.Has<NavigationComponent>()) return false;

        ref var transform = ref entity.Get<NavigationComponent>();
        transform.Target = new Vector2(x, y);
        return true;
    }

    private static bool IsNavigating(Entity entity)
    {
        return entity.Has<NavigationComponent>() && entity.Get<NavigationComponent>().Target.HasValue;
    }
}