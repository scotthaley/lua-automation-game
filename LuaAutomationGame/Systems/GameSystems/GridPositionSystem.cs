using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using LuaAutomationGame.Components.GameEngine;
using Microsoft.Xna.Framework;

namespace LuaAutomationGame.Systems.GameSystems;

public class GridPositionSystem(World world) : AEntitySetSystem<float>(world.GetEntities().With<TransformComponent>()
    .With<GridPositionComponent>().Without<NavigationComponent>().AsSet())
{
    protected override void Update(float state, in Entity entity)
    {
        ref var transform = ref entity.Get<TransformComponent>();
        ref var gridPosition = ref entity.Get<GridPositionComponent>();

        transform.Position = new Vector2(gridPosition.X * GameConstants.GridSize,
            gridPosition.Y * GameConstants.GridSize);
    }
}