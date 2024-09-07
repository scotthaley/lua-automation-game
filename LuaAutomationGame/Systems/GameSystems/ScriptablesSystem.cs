using System;
using System.Linq;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
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
                script.Globals["extract"] = (Func<bool>)(() => ExtractInventory(copiedEntity));
                script.Globals["is_extracting"] = (Func<bool>)(() => IsExtracting(copiedEntity));

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

    private static bool ExtractInventory(Entity entity)
    {
        if (!entity.Has<InventoryComponent>()) return false;
        if (!entity.Has<GridPositionComponent>()) return false;

        ref var inventory = ref entity.Get<InventoryComponent>();
        var gridPosition = entity.Get<GridPositionComponent>();

        var entitiesAtPosition =
            entity.World.GetEntities().With<InventoryComponent>().AsMultiMap<GridPositionComponent>()[gridPosition]
                .ToArray();

        var entitiesToCheck = entitiesAtPosition.Where(e => e != entity).ToArray();
        if (entitiesToCheck.Length == 0) return false;

        var inventoryEntity = entitiesToCheck.First();
        inventory.ExtractionEntity = inventoryEntity;
        inventory.IsExtracting = true;

        return true;
    }

    private static bool IsExtracting(Entity entity)
    {
        return entity.Has<InventoryComponent>() && entity.Get<InventoryComponent>().IsExtracting;
    }
}