public class Checkout : ICheckout
{
    private readonly List<Guid> _scannedItems = new();
    private readonly IProductRepository _productRepository;

    public Checkout(
        IProductRepository productRepository,
        DiscountService discountService
      )
    {
        _productRepository = productRepository;
    }

    public async Task ScanAsync(Guid productId)
    {
        _scannedItems.Add(productId);
    }

    public async Task<int> GetTotalPriceAsync()
    {
        decimal total = 0;
        foreach (var id in _scannedItems)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                total += product.UnitPrice;
            }
        }
        return (int)total;
    }
}
