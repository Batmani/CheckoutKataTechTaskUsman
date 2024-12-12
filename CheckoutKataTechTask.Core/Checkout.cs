public class Checkout : ICheckout
{

    private readonly List<Guid> _scannedItems = new();
    private readonly IProductRepository _productRepository;
    private readonly IDiscountService _discountService;
   // private readonly ILogger<Checkout> _logger;

    public Checkout(
        IProductRepository productRepository,
        IDiscountService discountService)
       // ILogger<Checkout> logger)
    {
        _productRepository = productRepository;
        _discountService = discountService;
    }

    public async Task ScanAsync(Guid productId)
    {
        var product = await _productRepository.GetByIdAsync(productId);
        if (product == null)
        {
            // _logger.LogWarning("Attempted to scan invalid product ID: {ProductId}", productId);
            throw new ArgumentException($"Invalid product ID: {productId}");
        }

        _scannedItems.Add(productId);
        //_logger.LogInformation("Scanned product: {ProductCode}", product.Code);
    }

    public async Task<int> GetTotalPriceAsync()
    {
        decimal total = 0;

        foreach (var productId in _scannedItems)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product != null)
            {
                total += product.UnitPrice;
            }
        }

        var discount = await _discountService.CalculateDiscountAsync(_scannedItems);
        total -= discount;

        return (int)Math.Round(total, MidpointRounding.AwayFromZero);
    }

    public void Clear()
    {
        _scannedItems.Clear();
       // _logger.LogInformation("Checkout cleared");
    }
}
