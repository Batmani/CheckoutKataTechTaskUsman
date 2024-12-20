﻿public class CheckoutTests
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

    [Fact]
    public async Task WhenScanningItemsWithBulkDiscount_ShouldApplyDiscount()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var repository = new InMemoryProductRepository();
        await repository.AddAsync(new Product(productId, "A", 50, "Product A"));

        var discountService = new DiscountService(repository);
        discountService.AddDiscountRule(
            new DiscountRule(productId, new BulkDiscountStrategy(3, 130)));

        var checkout = new Checkout(repository, discountService );

        // Act
        await checkout.ScanAsync(productId);
        await checkout.ScanAsync(productId);
        await checkout.ScanAsync(productId);
        var total = await checkout.GetTotalPriceAsync();

        // Assert
        Assert.Equal(130, total);
    }

    [Fact]
    public async Task WhenScanningInvalidProductId_ShouldThrowException()
    {
        // Arrange
        var repository = new InMemoryProductRepository();
        var checkout = new Checkout(repository,
            new DiscountService(repository) );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            async () => await checkout.ScanAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task WhenScanningMixedItems_ShouldApplyDiscountsCorrectly()
    {
        // Arrange
        var productAId = Guid.NewGuid();
        var productBId = Guid.NewGuid();
        var repository = new InMemoryProductRepository();

        await repository.AddAsync(new Product(productAId, "A", 50, "Product A"));
        await repository.AddAsync(new Product(productBId, "B", 30, "Product B"));

        var discountService = new DiscountService(repository);
        discountService.AddDiscountRule(
            new DiscountRule(productAId, new BulkDiscountStrategy(3, 130)));
        discountService.AddDiscountRule(
            new DiscountRule(productBId, new BulkDiscountStrategy(2, 45)));

        var checkout = new Checkout(repository, discountService);

        // Act
        await checkout.ScanAsync(productAId); // 50
        await checkout.ScanAsync(productBId); // 30
        await checkout.ScanAsync(productAId); // 50
        await checkout.ScanAsync(productBId); // 15 (45 for 2)
        await checkout.ScanAsync(productAId); // 30 (130 for 3)
        var total = await checkout.GetTotalPriceAsync();

        // Assert
        Assert.Equal(175, total); // 130 (3 A's) + 45 (2 B's)
    }
}

