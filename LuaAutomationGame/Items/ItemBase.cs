namespace LuaAutomationGame.Items;

public class ItemBase
{
    public ItemBase(string name, string description, int quantity = 1)
    {
        Name = name;
        Description = description;
        Quantity = quantity;
    }

    public ItemBase(ItemBase item, int? quantity = null)
    {
        Name = item.Name;
        Description = item.Description;
        Quantity = quantity ?? item.Quantity;
    }

    public string Name { get; }
    public string Description { get; }
    public int Quantity { get; set; }
}