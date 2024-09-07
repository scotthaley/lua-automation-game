using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using LuaAutomationGame.Components.GameEngine;

namespace LuaAutomationGame.Systems.GameSystems;

public class NavigationSystem(World world)
    : AEntitySetSystem<float>(world.GetEntities().With<NavigationComponent>().With<TransformComponent>().AsSet())
{
    protected override void Update(float state, in Entity entity)
    {
        ref var navigation = ref entity.Get<NavigationComponent>();
        if (!navigation.Target.HasValue) return;

        var target = navigation.Target.Value * GameConstants.GridSize;
        var direction = target - entity.Get<TransformComponent>().Position;
        var distance = direction.LengthSquared();
        direction.Normalize();
        entity.Get<TransformComponent>().Position += direction * navigation.Speed * state;

        // distance from target at which we need to start slowing down to hit target
        var timeToStop = navigation.Speed / navigation.Acceleration;
        var slowDistance = 0.5f * navigation.Speed * timeToStop;
        if (distance < slowDistance * slowDistance)
        {
            if (navigation.Speed > 0)
            {
                navigation.Speed -= navigation.Acceleration * state;
                if (navigation.Speed < 0) navigation.Speed = 0;
            }
        }
        else if (navigation.Speed < navigation.TopSpeed)
        {
            navigation.Speed += navigation.Acceleration * state;
        }

        if (distance < 0.1f)
        {
            entity.Get<TransformComponent>().Position = target;
            navigation.Target = null;
            navigation.Speed = 0;

            entity.Set(new GridPositionComponent
                { X = (int)(target.X / GameConstants.GridSize), Y = (int)(target.Y / GameConstants.GridSize) });
        }
    }
}