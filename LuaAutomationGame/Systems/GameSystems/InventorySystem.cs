using System.Linq;
using DefaultEcs;
using DefaultEcs.System;
using LuaAutomationGame.Components.Core;
using LuaAutomationGame.Components.GameEngine;
using LuaAutomationGame.Items;

namespace LuaAutomationGame.Systems.GameSystems;

public class InventorySystem(World world)
    : AEntitySetSystem<float>(world.GetEntities().With<InventoryComponent>().With<GridPositionComponent>().AsSet())
{
    private const float ExtractionTime = 3.0f;
    private float _extractionTime;

    protected override void Update(float state, in Entity entity)
    {
        ref var inventory = ref entity.Get<InventoryComponent>();

        if (!inventory.IsExtracting) return;
        var gridPosition = entity.Get<GridPositionComponent>();

        if (inventory.ExtractionEntity == null)
        {
            _extractionTime = 0.0f;
            inventory.IsExtracting = false;
            inventory.ExtractionEntity = null;
            return;
        }

        if (!inventory.ExtractionEntity.Value.IsAlive)
        {
            _extractionTime = 0.0f;
            inventory.IsExtracting = false;
            inventory.ExtractionEntity = null;
            return;
        }

        var extractionGridPosition = inventory.ExtractionEntity.Value.Get<GridPositionComponent>();
        if (!gridPosition.Equals(extractionGridPosition))
        {
            _extractionTime = 0.0f;
            inventory.IsExtracting = false;
            inventory.ExtractionEntity = null;
            return;
        }

        _extractionTime += state;

        inventory.ExtractionProgress = _extractionTime / ExtractionTime;
        if (!(_extractionTime >= ExtractionTime)) return;

        ref var extractionInventory = ref inventory.ExtractionEntity.Value.Get<InventoryComponent>();
        var item = extractionInventory.Items.FirstOrDefault();
        if (item != null)
        {
            item.Quantity--;

            var existingItem = inventory.Items.FirstOrDefault(i => i.Name == item.Name);
            if (existingItem != null)
                existingItem.Quantity++;
            else
                inventory.Items.Add(new ItemBase(item, 1));
        }

        _extractionTime = 0.0f;
        inventory.IsExtracting = false;
        inventory.ExtractionEntity = null;
    }
}