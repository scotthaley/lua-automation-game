using DefaultEcs;
using LuaAutomationGame.Components.Core;

namespace LuaAutomationGame.Components.Managers;

// maybe not needed?
public class MapManager
{
    private readonly EntityMultiMap<GridPositionComponent> _gridMap;

    public MapManager(World world)
    {
        _gridMap = world.GetEntities().AsMultiMap<GridPositionComponent>();
    }

    public Entity[] GetEntitiesAtPosition(int x, int y)
    {
        return _gridMap[new GridPositionComponent { X = x, Y = y }].ToArray();
    }
}