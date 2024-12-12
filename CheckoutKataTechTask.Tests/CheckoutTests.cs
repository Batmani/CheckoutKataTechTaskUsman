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
}