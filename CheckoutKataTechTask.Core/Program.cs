
internal class Program
{
    private static async Task Main(string[] args)
    {
        var productRepository = new InMemoryProductRepository();
        var discountService = new DiscountService(productRepository);

        var checkout = new Checkout(productRepository, discountService);

        // Add products
        var productId = Guid.NewGuid();
        await productRepository.AddAsync(new Product(productId, "A", 50, "Product A"));

        // Add discount rules
        discountService.AddDiscountRule(
            new DiscountRule(productId, new BulkDiscountStrategy(3, 130)));

        // Use checkout
        try
        {
            List<Guid> scannedItems = new List<Guid>() { productId, productId, productId };

            await checkout.ScanMultipleAsync(scannedItems);
            var total = await checkout.GetTotalPriceAsync();
            Console.WriteLine($"Total price: {total}");
        }
        catch (Exception ex)
        {
          //  logger.LogInformation("An error occurred during checkout");
        }
    }
}

