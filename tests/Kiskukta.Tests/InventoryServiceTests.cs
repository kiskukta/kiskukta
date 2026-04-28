using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Text.Json;
using Kiskukta.Services;
using Kiskukta.Interfaces;
using Kiskukta.Models;

[TestFixture]
public class InventoryServiceTests
{
    private Mock<IInventoryApi> _apiMock;
    private InventoryService _service;

    [SetUp]
    public void Setup()
    {
        _apiMock = new Mock<IInventoryApi>();
        _service = new InventoryService(_apiMock.Object);
    }

    [Test]
    public void GetInventory_Success_ReturnsProductList()
    {
        // Arrange
        var expectedProducts = new List<Product>
        {
            new Product { Id = 1, Name = "Milk", Stock = 10 },
            new Product { Id = 2, Name = "Bread", Stock = 5 }
        };

        var json = JsonSerializer.Serialize(expectedProducts);

        _apiMock.Setup(a => a.GetInventoryJson())
                .Returns(json);

        // Act
        var result = _service.GetInventory();

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Count > 0);
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual("Milk", result[0].Name);
    }
}
[TestFixture]
public class InventoryServiceStockTests
{
    private InventoryService _service;

    [SetUp]
    public void Setup()
    {
        _service = new InventoryService(null); // nincs API kell ehhez a teszthez
    }

    [Test]
    public void UpdateStock_DecreaseStock_ReturnsCorrectValue()
    {
        // Arrange
        int currentStock = 10;
        int delta = -3;

        // Act
        var result = _service.UpdateStock(currentStock, delta);

        // Assert
        Assert.AreEqual(7, result);
    }

    [Test]
    public void UpdateStock_NegativeResult_ShouldThrowException()
    {
        // Arrange
        int currentStock = 2;
        int delta = -5;

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            _service.UpdateStock(currentStock, delta);
        });
    }
}
