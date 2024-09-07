using System.Collections.Generic;
using DefaultEcs;
using LuaAutomationGame.Items;

namespace LuaAutomationGame.Components.GameEngine;

public struct InventoryComponent
{
    public int MaxItems { get; set; }
    public List<ItemBase> Items { get; set; }
    public bool IsExtracting { get; set; }
    public float ExtractionProgress { get; set; }
    public Entity? ExtractionEntity { get; set; }

    public InventoryComponent(int maxItems)
    {
        MaxItems = maxItems;
        Items = [];
    }
}