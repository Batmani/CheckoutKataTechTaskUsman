public class CheckoutTests
{
    [Fact]
    public async Task WhenScanningOneItem_ShouldReturnCorrectPrice()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new Product(productId, "A", 50, "Product A");
        var repository = new InMemoryProductRepository();
        await repository.AddAsync(product);

        var checkout = new Checkout(repository,
            new DiscountService(repository)
           );

        // Act
        await checkout.ScanAsync(productId);
        var total = await checkout.GetTotalPriceAsync();

        // Assert
        Assert.Equal(50, total);
    }


    [Fact]
    public async Task WhenScanningMultipleItems_ShouldReturnCorrectTotal()
    {
        // Arrange
        var productAId = Guid.NewGuid();
        var productBId = Guid.NewGuid();
        var repository = new InMemoryProductRepository();

        await repository.AddAsync(new Product(productAId, "A", 50, "Product A"));
        await repository.AddAsync(new Product(productBId, "B", 30, "Product B"));

        var checkout = new Checkout(repository,
            new DiscountService(repository) 
           );

        // Act
        await checkout.ScanAsync(productAId);
        await checkout.ScanAsync(productBId);
        var total = await checkout.GetTotalPriceAsync();

        // Assert
        Assert.Equal(80, total);
    }
}