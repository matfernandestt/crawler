using System.Collections.Generic;

public class Inventory
{
    private readonly Dictionary<ItemData, int> _items = new();

    public IEnumerable<ItemData> Items => _items.Keys;

    public int GetAmount(ItemData item)
    {
        if (item == null)
            return 0;

        return _items.TryGetValue(item, out var amount)
            ? amount
            : 0;
    }

    public void Add(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return;

        if (_items.ContainsKey(item))
            _items[item] += amount;
        else
            _items[item] = amount;
    }

    public bool TryUse(ItemData item)
    {
        if (GetAmount(item) <= 0)
            return false;

        _items[item]--;

        return true;
    }
}