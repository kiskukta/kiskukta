using System;
using Kiskukta.Interfaces;
using Kiskukta.Models;
using System.Text.Json;

public class InventoryService
{
    private readonly IInventoryApi _api;

    public InventoryService(IInventoryApi api)
    {
        _api = api;
    }

    public List<Product> GetInventory()
    {
        var json = _api.GetInventoryJson();
        return JsonSerializer.Deserialize<List<Product>>(json);
    }

    public int UpdateStock(int currentStock, int delta)
    {
        var newValue = currentStock + delta;
        if (newValue < 0)
            throw new InvalidOperationException("Negative stock not allowed");

        return newValue;
    }
}
