namespace LuaAutomationGame.Components.Core;

public struct GridPositionComponent
{
    public int X { get; set; }
    public int Y { get; set; }

    public bool Equals(GridPositionComponent other)
    {
        return X == other.X && Y == other.Y;
    }
}